/// <reference path="../_references.ts" />
module Account {
    export class LoginFormController {
        static $inject = ['$scope', 'commandBus', 'dialog', 'apiService', 'config', 'eventBus'];

        constructor(private $scope: ILoginFormControllerScope, commandBus: Core.ICommandBus, dialog: UI.IDialog, private apiService: Core.IApiService, private config: Core.IConfig, private eventBus: Core.IEventBus) {
            $scope.emailOrLogin = '';
            $scope.password = '';
            $scope.wrongLoginOrPassword = false;
            $scope.isLoginMode = true;
            $scope.isFocused = true;

            $scope.processLogin = url => {
                if ($scope.loginForm.$invalid) {
                    return;
                }

                var command = commandBus.createCommandFromScope(MirGames.Domain.LoginCommand, $scope);
                $scope.isFocused = false;

                commandBus.executeCommand(url, command, response => {
                    $scope.wrongLoginOrPassword = response.result === 0;

                    if (response.result === 1) {
                        this.eventBus.emit('ajax-request.executing');
                        window.location.reload();
                    } else {
                        $scope.isFocused = true;
                    }

                    $scope.$apply();
                });
            }

            $scope.close = () => {
                dialog.close(false);
            }

            $scope.restorePassword = this.restorePassword.bind(this);
            $scope.processRestore = this.processRestorePassword.bind(this);
            $scope.auth = provider => this.auth(provider);
        }

        private auth(provider: string) {
            var link = Router.action('OAuth', 'Authorize', { provider: provider });
            
            this.eventBus.emit('ajax-request.executing');
            $('<form action="' + link + '" method="POST"><input type="hidden" name="__RequestVerificationToken" value="' + this.config.antiForgery + '"></form>').submit();
        }

        private processRestorePassword(): void {
            var command: MirGames.Domain.Users.Commands.RequestPasswordRestoreCommand = {
                EmailOrLogin: this.$scope.emailOrLogin,
                NewPasswordHash: MD5(this.$scope.password)
            };

            this.apiService.executeCommand('RequestPasswordRestoreCommand', command, (result) => {
                if (result) {
                    this.$scope.$apply(() => {
                        this.$scope.restoreRequestSent = true;
                    });
                }
            });
        }

        private restorePassword(): void {
            this.$scope.isLoginMode = false;
            this.$scope.isRestoreMode = true;
            this.$scope.isFocused = true;
        }
    }

    export interface ILoginFormControllerScope extends ng.IScope {
        emailOrLogin: string;
        password: string;
        isFocused: boolean;
        loginForm: ng.IFormController;
        isLoginMode: boolean;
        isRestoreMode: boolean;

        wrongLoginOrPassword: boolean;
        restoreRequestSent: boolean;
        wrongLogin: boolean;

        auth(provider: string): void;
        processLogin(url: string): void;
        processRestore(): void;
        restorePassword(): void;
        close(): void;
    }
}