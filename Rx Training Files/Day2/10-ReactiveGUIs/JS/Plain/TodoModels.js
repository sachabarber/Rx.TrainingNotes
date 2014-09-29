(function (ko, adaptive) {
    //Very cool JS GUID generator from http://stackoverflow.com/questions/105034/how-to-create-a-guid-uuid-in-javascript
    // Not guaranteeing that it is production quality. -LC
    var guid = function () {
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) { var r = Math.random() * 16 | 0, v = c == 'x' ? r : r & 0x3 | 0x8; return v.toString(16); });
    };

    ///Represents a ToDo item.
    var todo = function (id, title, isCompleted, isDirty) {
        var self = this;
        self.id = id;
        self.title = ko.observable(title);
        self.isCompleted = ko.observable(isCompleted);
        self.isDeleted = ko.observable(false);
        self.isEditing = ko.observable(false);
        self.isDirty = ko.observable(isDirty);

        self.title.subscribe(function (newValue) { self.isDirty(true); });
        self.isCompleted.subscribe(function (newValue) { self.isDirty(true); });
        self.isDeleted.subscribe(function (newValue) { self.isDirty(true); });
    };

    //The main model for our ToDo list.
    var todoList = function (repository) {
        var self = this;
        self.repository = repository;
        self.items = ko.observableArray();
        self.isProcessing = ko.observable(false);

        var receiveItem = function (itemDto) {
			console.log('Receiving ...');
			console.log(itemDto);
			
            if (itemDto.isDeleted) {
				console.log('Delete');
                self.items.remove(function (x) { return x.id == itemDto.id; });				
            } else {
				var existing = ko.utils.arrayFirst(self.items(), function (x) { return x.id == itemDto.id; });
				if(existing)
				{
					console.log('update');
					existing.title(itemDto.title);
					existing.isCompleted(itemDto.isCompleted);
				}else{
					console.log('insert');
					var item = new todo(itemDto.id, itemDto.title, itemDto.isCompleted, false);
					self.items.push(item);
				}				
            }
        }

		var load = function () {
            self.repository.getTodoEvents.subscribe(receiveItem);
        };
		
        var save = function (item) {
            self.isProcessing(true);
			item.isEditing(false);
			var dto = ko.toJS(item);
			console.log('Saving ...');
			console.log(dto);
            self.repository.saveTodo(dto)
                .subscribe(function () {
                    self.isProcessing(false);
                });
        }

		var remove = function(item) {
			item.isDeleted(true);
			save(item);
		};
        

        var createNew = function (title) {			
			if(title==null || title=='')
				return;
            var item = new todo(guid(), title, false, false, false);
            save(item);
        };

        self.load = load;
        self.createNew = createNew;
        self.remove = function() {
			remove(this);
		}
		self.save = function() {
			save(this);
		};
		
    };

    adaptive.todoList = todoList;

    // ReSharper disable ThisInGlobalContext
}(ko, this.adaptive = this.adaptive || {}));
// ReSharper restore ThisInGlobalContext