mergeInto(LibraryManager.library, {

  CreateUserWithEmailAndPassword: function (email, password, objectName, callback, fallback) {
      var parsedEmail = UTF8ToString(email);
      var parsedPassword = UTF8ToString(password);
      var parsedObjectName = UTF8ToString(objectName);
      var parsedCallback = UTF8ToString(callback);
      var parsedFallback = UTF8ToString(fallback);

      try {
        var { auth, createUserWithEmailAndPassword } = window.firebaseAuth;

        createUserWithEmailAndPassword(auth, parsedEmail, parsedPassword).then(function (unused) {
              window.unityInstance.SendMessage(parsedObjectName, parsedCallback, "Success: signed up for " + parsedEmail);
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

          window.firebaseAuth.signInWithEmailAndPassword(parsedEmail, parsedPassword).then(function (unused) {
              window.unityInstance.SendMessage(parsedObjectName, parsedCallback, "Success: signed in for " + parsedEmail);
          }).catch(function (error) {
              window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
          });

      } catch (error) {
          window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
      }
  },

  SignInWithGoogle: function (objectName, callback, fallback) {
      var parsedObjectName = UTF8ToString(objectName);
      var parsedCallback = UTF8ToString(callback);
      var parsedFallback = UTF8ToString(fallback);

      try {
          var provider = new window.firebaseAuth.GoogleAuthProvider();
          window.firebaseAuth.auth().signInWithRedirect(provider).then(function (unused) {
              window.unityInstance.SendMessage(parsedObjectName, parsedCallback, "Success: signed in with Google!");
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

      var { auth } = window.firebaseAuth;
      console.log(auth);
      auth.onAuthStateChanged(function(user) {
          if (user) {
              window.unityInstance.SendMessage(parsedObjectName, parsedOnUserSignedIn, JSON.stringify(user));
          } else {
              window.unityInstance.SendMessage(parsedObjectName, parsedOnUserSignedOut, "User signed out");
          }
      });

  }
});