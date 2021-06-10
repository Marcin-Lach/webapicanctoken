# Fun with CancellationToken in WebApi

There are 3 endpoints. Only one handles HTTP Request CancellationToken and in case of user aborting HTTP Request, breaks execution before persisting operation result.

In Visual Studio - Run application in debug mode using local IIS. Watch console after visiting any of links below. Do not use Swagger to send and abort requests - Swagger does not abort requests. Open below links in web browser and press abort button.

If you go to <https://localhost:44320/CancelToken/checktoken> and abort immediately, you should see exception in console which will be handled on controller level.

If you go to <https://localhost:44320/CancelToken/notchecktoken> and abort immediately, you will see info about persisting operation result - this means that cancellationToken has not been checked

If you go to <https://localhost:44320/CancelToken/nothttprequesttoken> and abort immediately, you will see info about persisting operation result - this means that CancellationTokenSource is not the same as for HTTP Request Cancellation Token

If you go to <https://localhost:44320/CancelToken/checktokenandpasstoothermethods> and abort while LongRunningOperation is running (logging stuff), this will stop this operation

## Troubleshooting

If you run this through dotnet cli, change ports in above links to match your setup (probably 5000 for HTTP and 5001 for HTTPS)
