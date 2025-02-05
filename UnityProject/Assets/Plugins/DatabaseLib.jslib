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
  }
  
});