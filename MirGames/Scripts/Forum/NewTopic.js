var MirGames;
(function (MirGames) {
    /// <reference path="../_references.ts" />
    (function (Forum) {
        var NewTopicPage = (function () {
            function NewTopicPage($scope, $element, commandBus, eventBus) {
                var _this = this;
                this.$scope = $scope;
                this.$element = $element;
                this.commandBus = commandBus;
                this.eventBus = eventBus;
                this.$scope.post = this.submit.bind(this);
                this.$scope.attachments = [];
                this.$scope.switchPreviewMode = function () {
                    _this.$scope.showPreview = !_this.$scope.showPreview;
                };
                this.$scope.isTitleFocused = true;
            }
            NewTopicPage.prototype.submit = function () {
                var command = this.commandBus.createCommandFromScope(MirGames.Domain.PostNewForumTopicCommand, this.$scope);

                this.commandBus.executeCommand(Router.action("Forum", "PostNewTopic"), command, function (result) {
                    Core.Application.getInstance().navigateToUrl(Router.action("Forum", "Topic", { topicId: result.topicId }));
                });
            };
            NewTopicPage.$inject = ['$scope', '$element', 'commandBus', 'eventBus'];
            return NewTopicPage;
        })();
        Forum.NewTopicPage = NewTopicPage;
    })(MirGames.Forum || (MirGames.Forum = {}));
    var Forum = MirGames.Forum;
})(MirGames || (MirGames = {}));
//# sourceMappingURL=NewTopic.js.map
