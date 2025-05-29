mergeInto(LibraryManager.library, {

    CreateUserWithEmailAndPassword: function (email, password, objectName, callback, fallback) {
        var parsedEmail = UTF8ToString(email);
        var parsedPassword = UTF8ToString(password);
        var parsedObjectName = UTF8ToString(objectName);
        var parsedCallback = UTF8ToString(callback);
        var parsedFallback = UTF8ToString(fallback);

        try {
        window.firebaseAuth.createUserWithEmailAndPassword(window.firebaseAuth.auth, parsedEmail, parsedPassword).then(function (userCredential) {
                window.unityInstance.SendMessage(parsedObjectName, parsedCallback, JSON.stringify(userCredential.user));
            }).catch(function (error) {
                window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
            });

        } catch (error) {
            window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },

    SignInWithEmailAndPassword: function (email, password, objectName, callback, fallback) {
        var parsedEmail = UTF8ToString(email);
        var parsedPassword = UTF8ToString(password);
        var parsedObjectName = UTF8ToString(objectName);
        var parsedCallback = UTF8ToString(callback);
        var parsedFallback = UTF8ToString(fallback);

        try {

            window.firebaseAuth.signInWithEmailAndPassword(window.firebaseAuth.auth, parsedEmail, parsedPassword).then(function (userCredential) {
                window.unityInstance.SendMessage(parsedObjectName, parsedCallback, JSON.stringify(userCredential.user));
            }).catch(function (error) {
                window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
            });

        } catch (error) {
            window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },

    OnAuthStateChanged: function (objectName, onUserSignedIn, onUserSignedOut) {
        var parsedObjectName = UTF8ToString(objectName);
        var parsedOnUserSignedIn = UTF8ToString(onUserSignedIn);
        var parsedOnUserSignedOut = UTF8ToString(onUserSignedOut);

        window.firebaseAuth.onAuthStateChanged(window.firebaseAuth.auth, function(user) {
            if (user) {
                window.unityInstance.SendMessage(parsedObjectName, parsedOnUserSignedIn, JSON.stringify(user));
            } else {
                window.unityInstance.SendMessage(parsedObjectName, parsedOnUserSignedOut, "User signed out");
            }
        });
    },

    OnUserSignOut: function (objectName, onUserSignedOut) {
        var parsedObjectName = UTF8ToString(objectName);
        var parsedOnUserSignedOut = UTF8ToString(onUserSignedOut);

        window.firebaseAuth.signOut(window.firebaseAuth.auth)
            .then(function () {
                window.unityInstance.SendMessage(parsedObjectName, parsedOnUserSignedOut, "User signed out");
            })
            .catch(function (error) {
                window.unityInstance.SendMessage(parsedObjectName, parsedOnUserSignedOut, "Sign-out failed: " + error.message);
            });
    }
});