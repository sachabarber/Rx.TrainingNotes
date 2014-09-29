(function (Rx, adaptive) {
    var repository = function(){
		var self = this;
		var changes = new Rx.Subject();
		
		var save = function(item){
			return Rx.Observable.create(function (observer) {
				changes.onNext(item);
				observer.onNext();
				observer.onCompleted();
				return function () {};
			});
		};
		
		self.saveTodo = save;
		self.getTodoEvents = changes;
	};

    adaptive.stubTodoRepo = repository;

    // ReSharper disable ThisInGlobalContext
}(Rx, this.adaptive = this.adaptive || {}));
// ReSharper restore ThisInGlobalContext