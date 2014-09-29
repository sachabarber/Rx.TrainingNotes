(function ($, ko, adaptive) {
	var addBlurOnEnterKeyBinding = function(){
		
		ko.bindingHandlers.blurOnEnter = { 
			init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
				ko.utils.registerEventHandler(element, 'keydown', function(evt) {
					if (evt.keyCode === 13)
						$(element).blur();
				});
			}
		};
	};

	var addDoubleClickBinding = function() {
		ko.bindingHandlers.doubleClick= {
			init: function(element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
				var handler = valueAccessor(),
					delay = 200,
					clickTimeout = false;

				$(element).click(function() {
					if(clickTimeout !== false) {
						handler.call(viewModel);
						clickTimeout = false;
					} else {        
						clickTimeout = setTimeout(function() {
							clickTimeout = false;
						}, delay);
					}
				});
			}
		};
	};

    adaptive.createCustomKoBindings = function () {
        addBlurOnEnterKeyBinding();
        addDoubleClickBinding();
    };
// ReSharper disable ThisInGlobalContext
}(jQuery, ko, this.adaptive = this.adaptive || {}));
// ReSharper restore ThisInGlobalContext