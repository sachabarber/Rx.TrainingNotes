Rx.Observable.prototype.log = function (sourceName) {
    var source = this;

    return Rx.Observable.create(function (observer) {
        console.log(sourceName + '.Subscribe()');

        var disposal = Rx.Disposable.create(function () {
            console.log(sourceName + '.Dispose()');
        });

        var subscription = source.do(
                function (x) { console.log(sourceName + '.onNext(' + x + ')'); },
                function (err) { console.log(sourceName + '.onError(' + err + ')'); },
                function () { console.log(sourceName + '.onCompleted()'); }
            )
            .subscribe(observer);

        return new Rx.CompositeDisposable(disposal, subscription);
    });
};

(function (Rx, adaptive) {
    adaptive.Db = {};
    //PouchDB.destroy('adative.practicalRx.todoTasks');
    var todoDb = new PouchDB('adative.practicalRx.todoTasks');
    
    var getHeadVersion = function () {
        return Rx.Observable.createWithDisposable(function (o) {
            var query = todoDb.allDocs({ include_docs: true, descending: true, limit: 1 }, function (err, response) {                
                if (err) {
                    console.error('Could not retrieve head task');
                    o.onError(err);
                } else if (response) {
                    if (response.total_rows == 0) {
                        o.onNext(-1);
                        o.onCompleted();
                    }
                    else {
                        o.onNext(response.rows[0].doc.Version);
                        o.onCompleted();
                    }
                }
            });
            
            return function () { query.cancel(); };
        });
    }
    var getAllUpdates = function () {
        return Rx.Observable.createWithDisposable(function (o) {

            var query = todoDb.info(function (err, info) {
                todoDb.changes({
                    since: 0,
                    live: true,
                    include_docs: true,
                }).on('change', function (change) {
                    o.onNext(change.doc);
                });
            });

            return function () { query.cancel(); };
        });
    };
    var persistTask = function (todoUpdate) {
        todoUpdate['_id'] = todoUpdate.EventId;
        console.log('Persisting update : ' + todoUpdate.EventId);
        todoDb.put(todoUpdate, function (err, result) {
            if (err) {
                console.log('Could not save taskUpdate');
                console.log(todoUpdate);
                console.error(err);
            }
        });
    };

    adaptive.Db.getHeadVersion = getHeadVersion;
    adaptive.Db.getAllUpdates = getAllUpdates;
    adaptive.Db.persistTask = persistTask;
    
    // ReSharper disable ThisInGlobalContext
}(Rx, this.adaptive = this.adaptive || {}));
// ReSharper restore ThisInGlobalContext

(function (Rx, adaptive) {
    var signalRTodoRepository = function () {
        var self = this;
        self.currentVersion = -1;
        var commandHub = $.connection.taskCommandHub;
        var queryHub = $.connection.taskQueryHub;

        function taskUpdates() {
            return Rx.Observable.createWithDisposable(function (o) {
                
                var localHeadVersion = -1;
                var serverHeadVersion = -1;
                
                queryHub.client.taskUpdates = function (data) {
                    console.log('currentVersion  from ' + self.currentVersion + '=>' + data.Version + '');
                    self.currentVersion = data.Version;
                    data['headVersion'] = serverHeadVersion > data.Version ? serverHeadVersion : data.Version;
                    adaptive.Db.persistTask(data);
                };
                var commandSubscription =
                    adaptive.Db.getHeadVersion().log('getLocalHeadVersion')
                    .do(function(headVersion) {
                            localHeadVersion = headVersion;
                            if (headVersion == -1) {
                                o.onNext({ Version: -1, headVersion: -1 });
                            }
                        })
                    .selectMany(hubStart).log('hubStart')
                    .selectMany(function () { return Rx.Observable.fromPromise(queryHub.server.getHeadVersion()); }).log('getServerHeadVersion')
                    .do(function (headVer) { serverHeadVersion = headVer; })
                    .selectMany(function () { return Rx.Observable.fromPromise(queryHub.server.getTaskUpdatesFrom(localHeadVersion)); }).log('getTaskUpdatesFrom')
                    .subscribe(
                        function () { },
                        function (error) { o.onError(error); });

                var dbSubscription = adaptive.Db.getAllUpdates()
                    .log('db.getAllUpdates')
                    .subscribe(
                        function (x) { o.onNext(x); },
                        function (error) { o.onError(error); },
                        function () { o.onCompleted(); });

                return new Rx.CompositeDisposable(commandSubscription, dbSubscription);
            }).log('taskUpdates');
        }

        function addItem(item) {
            var addCommand = {
                eventId: guid(),
                newTaskId: item.id,
                expectedVersion: self.currentVersion,
                isCompleted: item.isCompleted,
                title: item.title
            }
            return hubStart()
                .selectMany(function () { return Rx.Observable.fromPromise(commandHub.server.addTask(addCommand)); });

        }
        function updateItem(item) {
            var updateCommand = {
                eventId: guid(),
                taskId: item.id,
                expectedVersion: self.currentVersion,
                isCompleted: item.isCompleted,
                title: item.title
            }
            return hubStart()
                .selectMany(function () { return Rx.Observable.fromPromise(commandHub.server.updateTask(updateCommand)); });
        }
        function removeItem(item) {
            var deleteCommand = {
                eventId: guid(),
                taskId: item.id,
                expectedVersion: self.currentVersion
            }
            return hubStart()
                .selectMany(function () { return Rx.Observable.fromPromise(commandHub.server.deleteTask(deleteCommand)); });
        }

        function hubStart() {
            return Rx.Observable.fromPromise($.connection.hub.start())
                .select(function () { return $.connection.hub.state; })
                .startWith($.connection.hub.state)
                .where(function (currentState) { return currentState === $.signalR.connectionState.connected; })
                .take(1);
        }

        self.addItem = addItem;
        self.updateItem = updateItem;
        self.removeItem = removeItem;
        self.updates = taskUpdates(-1);
    };

    adaptive.signalRTodoRepo = signalRTodoRepository;

    // ReSharper disable ThisInGlobalContext
}(Rx, this.adaptive = this.adaptive || {}));
// ReSharper restore ThisInGlobalContext