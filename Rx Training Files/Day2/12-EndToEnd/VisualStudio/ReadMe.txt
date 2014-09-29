You will need to run the EventSource Server to for the end to end sample to work.
You can do this by opening a command prompt and running the ..\Server\StartServer.bat, or alternatively just double clicking on the batch file from window explorer.

If you want to clear out any data and start fresh, just stop the EventStore service (CTRL+C) and then delete the ..\Server\db and ..\Server\db-logs directories. Starting the server now should give you a fresh instance.