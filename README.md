# BookingBug
POC for booking bug api

Simple POC project pulls data from booking bug using the API then saves it to a database.

  \Source\IntegrationServices\Interfaces\IIntegration.cs(6):    //TODO: This really should probably go into contract or core or something like that.
  \Source\IntegrationDataServices\BookingRepository.cs(8):    //TODO: consider a more generic repository pattern?
  \Source\IntegrationDataServices\BookingRepository.cs(9):    //TODO: consider moving the interfaces into the contract project?
  \Source\IntegrationDataServices\BookingRepository.cs(14):        //TODO: this thing is in a few places and is a bit goofy?
  \Source\IntegrationDataServices\BookingRepository.cs(83):        //TODO: consider using automapper for this sort of stuff?
  \Source\BookingBugBookingIntegration\BookingBugApi.cs(13):    //TODO: the file generation really doesn't belong here, it's just convenient for debugging, consider doing it as some sort of subscription?
  \Source\BookingBugBookingIntegration\BookingBugApi.cs(14):    //TODO: Too long, split up? I like that all these silly json related strings are contrained to a couple classes.
  \Source\BookingBugBookingIntegration\BookingBugApi.cs(52):                //TODO: duplicate of above - refactor
  \Source\BookingBugBookingIntegration\BookingBugApi.cs(166):                //TODO: Try catch log?
  \Source\BookingBugBookingIntegration\BookingBugClient.cs(14):        //Todo: Consider introducing a layer of abstraction for the configuration manager.
  \Source\BookingBugBookingIntegration\BookingBugClient.cs(15):        //TODO: Consider a constants file for the keys? They're only really used one place, abstraction for the config manager might settle this issue.
  \Source\BookingBugBookingIntegration\BookingBugClient.cs(16):        //TODO: I've got a lot of strings running around in this file and the api file. I think I'm ok with that...but it does feel a bit awk.
  \Source\BookingBugBookingIntegration\ParameterHandler.cs(7):    //TODO: There's probably a better way to handle this but this is easy.
  
