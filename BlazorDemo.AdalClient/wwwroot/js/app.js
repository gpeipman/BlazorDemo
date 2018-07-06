var authContext = null;
var user = null;

(function () {
    window.config = {
        instance: 'https://login.microsoftonline.com/',
        tenant: 'gpeipmanhotmail.onmicrosoft.com',
        clientId: 'cbb1e71f-87f3-46bd-a2e6-ffdcd43865d9',
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

Blazor.registerFunction('confirmDelete', (title) => {
    $('#bookTitleField').text(title);
    $('#myModal').modal('show');

    return true;
});

Blazor.registerFunction('hideDeleteDialog', () => {
    $('#myModal').modal('hide');

    return true;
});

Blazor.registerFunction('getUserName', () => {
    if (user == null) {
        return '';
    }

    return user.profile.name;
});

Blazor.registerFunction('executeWithToken', (action) => {
    authContext.acquireToken(authContext.config.clientId, function (error, token) {
        let tokenString = Blazor.platform.toDotNetString(token);

        const assemblyName = 'BlazorDemo.AdalClient';
        const namespace = 'BlazorDemo.AdalClient';
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
});