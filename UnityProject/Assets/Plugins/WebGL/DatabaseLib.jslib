mergeInto(LibraryManager.library, {

  GetJSON: function (path, objectName, callback, fallback) {
    var parsedPath = UTF8ToString(path);
    var parsedObject = UTF8ToString(objectName);
    var parsedCallback = UTF8ToString(callback);
    var parsedFallback = UTF8ToString(fallback);

    try 
    {
      var { ref, get, database } = window.firebaseDatabase;
      var dbRef = ref(database, parsedPath);

      get(dbRef).then(function (snapshot) {
        if (snapshot.exists()) {
          window.unityInstance.SendMessage(parsedObject, parsedCallback, JSON.stringify(snapshot.val()));
        } else {
          window.unityInstance.SendMessage(parsedObject, parsedFallback, "No data available at path: " + parsedPath);
        }
      }).catch(function (error) {
        window.unityInstance.SendMessage(parsedObject, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
      });
    } 
    catch (error) 
    {
      window.unityInstance.SendMessage(parsedObject, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
    }
  },

  CreatePlayerDataIfNotExists: function (userToFetch /*path + userUiD*/, jsonData/*player data*/, objectName, callback, fallback) {
    var parseduserToFetch = UTF8ToString(userToFetch);
    var parsedData = UTF8ToString(jsonData);
    var parsedObject = UTF8ToString(objectName);
    var parsedCallback = UTF8ToString(callback);
    var parsedFallback = UTF8ToString(fallback);

    try {
      var { ref, get, set, database } = window.firebaseDatabase;
      var userRef = ref(database, parseduserToFetch);

      get(userRef).then(function (snapshot) {
        if (snapshot.exists()) {
          window.unityInstance.SendMessage(parsedObject, parsedCallback, "User already exists");
        } else {
          var dataObject = JSON.parse(parsedData);
          set(userRef, dataObject).then(function () {
            window.unityInstance.SendMessage(parsedObject, parsedCallback, "User created successfully");
          }).catch(function (error) {
            window.unityInstance.SendMessage(parsedObject, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
          });
        }
      }).catch(function (error) {
        window.unityInstance.SendMessage(parsedObject, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
      });

    } catch (error) {
      window.unityInstance.SendMessage(parsedObject, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
    }
  },

  FetchPlayerData: function (userToFetch, objectName, callback, fallback) {
    var parseduserToFetch = UTF8ToString(userToFetch);
    var parsedObject = UTF8ToString(objectName);
    var parsedCallback = UTF8ToString(callback);
    var parsedFallback = UTF8ToString(fallback);

    try {
      var { ref, get, database } = window.firebaseDatabase;
      var userRef = ref(database, parseduserToFetch);

      get(userRef).then(function (snapshot) {
        if (snapshot.exists()) {
          window.unityInstance.SendMessage(parsedObject, parsedCallback, JSON.stringify(snapshot.val()));
        } else {
          window.unityInstance.SendMessage(parsedObject, parsedFallback, "User not found");
        }
      }).catch(function (error) {
        window.unityInstance.SendMessage(parsedObject, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
      });

    } catch (error) {
      window.unityInstance.SendMessage(parsedObject, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
    }
  },

  UpdatePlayerData: function (userToUpdate, jsonData, objectName, callback, fallback) {
    var parsedUser = UTF8ToString(userToUpdate);
    var parsedData = UTF8ToString(jsonData);
    var parsedObject = UTF8ToString(objectName);
    var parsedCallback = UTF8ToString(callback);
    var parsedFallback = UTF8ToString(fallback);

    try {
      var { ref, update, database } = window.firebaseDatabase;
      var userRef = ref(database, parsedUser);

      var dataObject = JSON.parse(parsedData);

      update(userRef, dataObject).then(function () {
        window.unityInstance.SendMessage(parsedObject, parsedCallback, "User data updated successfully");
      }).catch(function (error) {
        window.unityInstance.SendMessage(parsedObject, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
      });

    } catch (error) {
      window.unityInstance.SendMessage(parsedObject, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
    }
  }
  
});