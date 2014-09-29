//Very cool JS GUID generator from http://stackoverflow.com/questions/105034/how-to-create-a-guid-uuid-in-javascript
// Not guaranteeing that it is production quality. -LC
var guid = function () {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) { var r = Math.random() * 16 | 0, v = c == 'x' ? r : r & 0x3 | 0x8; return v.toString(16); });
};

(function (ko, Rx, adaptive) {

    ///Represents a ToDo item.
    var todo = function (parent, id, title, isCompleted) {
        var self = this;

        self.id = id;
        self.title = ko.observable(title);
        self.isCompleted = ko.observable(isCompleted);
        self.isDeleted = ko.observable(false);
        self.isEditing = ko.observable(false);
        self.remove = function () { parent.requestRemove(self); }
        self.update = function (newTitle, newIsCompleted) {
            self.isAutoSaveSuppressed = true;
            self.title(newTitle);
            self.isCompleted(newIsCompleted);
            self.isAutoSaveSuppressed = false;
        }
        self.title.subscribe(function () {
            if (!self.isAutoSaveSuppressed) {
                parent.requestUpdate(self);
            }
        });
        self.isCompleted.subscribe(function () {
            if (!self.isAutoSaveSuppressed) {
                parent.requestUpdate(self);
            }
        });
    };

    //The main model for our ToDo list.
    var todoList = function (repository) {
        var self = this;
        self.repository = repository;
        self.items = ko.observableArray();
        self.isProcessing = ko.observable(false);

        var load = function () {
            self.isProcessing(true);

            self.repository.updates.subscribe(
                function (itemDto) {
                    if (itemDto.Version == itemDto.headVersion) {
                        self.isProcessing(false);
                    }
                    receiveItem(itemDto);
                },
                function (error) {
                    console.error(error);
                });

        };
        var receiveItem = function (itemDto) {
            console.log('Receiving ...');
            console.log(itemDto);

            if (itemDto.AddedEvent != null) {
                insert(itemDto);
            }
            else if (itemDto.UpdatedEvent != null) {
                update(itemDto);
            }
            else if (itemDto.DeletedEvent != null) {
                remove(itemDto.DeletedEvent.TaskId);
            }
        }

        var createNew = function (title) {
            if (title == null || title == '')
                return;
            var item = new todo(self, guid(), title, false);
            requestAdd(item);
        };

        var requestAdd = function (item) {
            self.isProcessing(true);
            item.isEditing(false);
            var dto = ko.toJS(item);
            console.log('Requesting Add ...');
            console.log(dto);
            self.repository.addItem(dto)
		        .subscribe(
                    function () { },
		            function (error) {
		                self.isProcessing(false);
		                console.error(error);
		            },
		            function () { self.isProcessing(false); });
        }
        var requestUpdate = function (item) {
            self.isProcessing(true);
            item.isEditing(false);
            var dto = ko.toJS(item);
            console.log('Requesting update ...');
            console.log(dto);
            self.repository.updateItem(dto)
                .subscribe(function () { },
                    function (error) {
                        self.isProcessing(false);
                        console.error(error);
                    },
		            function () { self.isProcessing(false); });
        }
        var requestRemove = function (item) {
            self.isProcessing(true);
            item.isDeleted(true);
            item.isEditing(false);
            var dto = ko.toJS(item);
            console.log('Requesting delete ...');
            console.log(dto);
            self.repository.removeItem(dto)
                .subscribe(function () { },
		            function (error) {
		                self.isProcessing(false);
		                console.error(error);
		            },
		            function () { self.isProcessing(false); });
        }

        function insert(itemDto) {
            var item = new todo(self, itemDto.AddedEvent.TaskId, itemDto.AddedEvent.Title, false);
            self.items.push(item);
        }
        function update(itemDto) {
            var existing = ko.utils.arrayFirst(self.items(), function (x) { return x.id == itemDto.UpdatedEvent.TaskId; });
            if (existing == null) {
                console.error("Expected TaskId:'" + itemDto.UpdatedEvent.taskId + "' but was missing");
            } else {
                existing.update(itemDto.UpdatedEvent.Title, itemDto.UpdatedEvent.IsCompleted);
            }
        }
        function remove(taskId) {
            self.items.remove(function (x) { return x.id == taskId; });
        }


        self.load = load;
        self.createNew = createNew;
        self.requestRemove = requestRemove;
        self.requestUpdate = requestUpdate;
    };

    adaptive.todoList = todoList;

    // ReSharper disable ThisInGlobalContext
}(ko, Rx, this.adaptive = this.adaptive || {}));
// ReSharper restore ThisInGlobalContext