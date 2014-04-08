var Config = {
    path: {
        root: 'http://localhost'
    }
};

var Topics;
(function (scope) {
    var FavoriteTopicsViewModel = function() {
        this.isShown = ko.observable(false);
        this.topics = ko.observableArray();

        this.connection = $.connection.topics;

        this.connection.client.favoritesLoaded = function (data) {
            this.topics($.map(data, function (item) {
                return new Topics.FavoriteTopicViewModel(item);
            }));
            this.isLoaded = true;
        }.bind(this);
    };
    
    FavoriteTopicsViewModel.prototype = {
        toggle: function () {
            if (this.isShown()) {
                this.hide();
            } else {
                this.show();
            }
        },
        show: function () {
            if (this.isShown()) {
                return;
            }
            this.load();
            this.isShown(true);
        },
        hide: function () {
            if (!this.isShown()) {
                return;
            }
            this.isShown(false);
        },
        load: function () {
            if (!this.isLoaded) {
                this.connection.server.loadFavorites();
            }
        }
    };
    
    scope.FavoriteTopicsViewModel = FavoriteTopicsViewModel;
})(Topics || (Topics = {}));

var Topics;
(function (scope) {
    var FavoriteTopicViewModel = function(topic) {
        this.name = topic.TopicTitle;
        this.url = Config.path.root + "/blog/" + [
            topic.BlogUrl,
            topic.Id.toString()
        ].filter(function(item) {
            return !!item;
        }).join('/') + ".html";
        this.date = new Date(topic.CreationDate);
    };

    scope.FavoriteTopicViewModel = FavoriteTopicViewModel;
})(Topics || (Topics = {}));

(function (scope) {
    scope.FavoritesToolbarItem = function(viewModel) {
        this.viewModel = viewModel;
    };
    scope.FavoritesToolbarItem.prototype = {
        init: function () {
            var _this = this;

            this.toolbarItem = $('#toolbar-topic-favorites');
            this.toolbarLink = $('#toolbar-topic-favorites i');

            this.favoritesTopics = $('<section class="block block-type-favorites" style="display: none; "><header class="block-header"><h3>Избранное</h3></header><div class="block-content"><ul data-bind="foreach: topics" class="favorites-topics-list"><li><a data-bind="text: name, attr: { href: url }"></a></li></ul></div></section>');

            $('#sidebar').append(this.favoritesTopics);

            ko.applyBindings(this.viewModel, this.favoritesTopics.get(0));
            this.toolbarLink.click(function() {
                return _this.viewModel.toggle();
            });

            this.viewModel.isShown.subscribe(function (newValue) {
                if (newValue) {
                    $('#sidebar > :visible').fadeOut("fast", function() {
                        return _this.favoritesTopics.fadeIn("fast");
                    });
                    _this.toolbarItem.addClass('selected');
                } else {
                    _this.favoritesTopics.fadeOut("fast", function() {
                        return $('#sidebar > :not(:visible)').not(_this.favoritesTopics).fadeIn("fast");
                    });
                    _this.toolbarItem.removeClass('selected');
                }
            });
        }
    };
})(Topics || (Topics = {}));