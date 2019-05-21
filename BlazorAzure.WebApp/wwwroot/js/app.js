var authContext = null;
var user = null;

(function () {
    window.config = {
        instance: 'https://login.microsoftonline.com/',
        tenant: '<YOUR TENANT ID HERE>',
        clientId: '<YOUR CLIENT ID HERE>',
        postLogoutRedirectUri: window.location.origin,
        cacheLocation: 'localStorage' // enable this for IE, as sessionStorage does not work for localhost.
    };

    authContext = new AuthenticationContext(config);
    var isCallback = authContext.isCallback(window.location.hash);
    authContext.handleWindowCallback();
    //$errorMessage.html(authContext.getLoginError());

    if (isCallback && !authContext.getLoginError()) {
        window.location = authContext._getItem(authContext.CONSTANTS.STORAGE.LOGIN_REQUEST);
    }

    // Check Login Status, Update UI
    user = authContext.getCachedUser();
    if (!user) {
        authContext.login();
    }

}());

window.blazorDemoInterop = {
    confirmDelete: function (title) {
        $('#bookTitleField').text(title);
        $('#myModal').modal('show');

        return true;
    },
    hideDeleteDialog: function () {
        $('#myModal').modal('hide');

        return true;
    },
    getUserName: function () {
        if (user === null) {
            return Blazor.platform.toDotNetString('');
        }

        return user.profile.name;
    },
    executeWithToken: function (action) {
        authContext.acquireToken(authContext.config.clientId, function (error, token) {
            let tokenString = Blazor.platform.toDotNetString(token);

            const assemblyName = 'BlazorAzure.WebApp';
            const namespace = 'BlazorAzure.WebApp';
            const typeName = 'AdalHelper';
            const methodName = 'RunAction';

            const runActionMethod = Blazor.platform.findMethod(
                assemblyName,
                namespace,
                typeName,
                methodName
            );

            Blazor.platform.callMethod(runActionMethod, null, [
                action, tokenString
            ]);

        });

        return true;
    }
};