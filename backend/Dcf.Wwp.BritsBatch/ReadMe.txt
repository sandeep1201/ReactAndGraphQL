-----------------------------------------------------------
To add a new batch job, create (or copy) a new class under 
\Models that implements IBatchJob. Set the appropriate
public properties, then register the implementation with
Autofac in the AutofacModule file. Test. That's it.
-----------------------------------------------------------
Typically the class you create should be named the same as 
the Control-M job name to make traceback easier (like knowing
which Ctrl-M job maps back to which .NET class). Purely
convention.
-----------------------------------------------------------
In your job classes, don't forget to include the appropriate
logging statements for what you're doing at the appropriate levels ;)
-----------------------------------------------------------
Exception handling: What's the difference between 'Error'
 and 'Fatal'?

 Basically, fatal errors are when your application can't do any more useful 
 work. Non-fatal errors are when there's a problem but the app can still 
 continue to function, albeit at a reduced level of functionality.

Fatal ex:
•Running out of disk space on the logging device and you're required to keep logging.
•Total loss of network connectivity in a client application.
•Missing configuration information if no default can be used (such as missing credentials).
-----------------------------------------------------------
The job output files are stored in their own directories because 
while the maximum number of files in a folder is 4,294,967,295, if 
you are planning to manipulate them using Windows Explorer then you 
will start to see very slow performance with as few as 16,000 files. 
So the separate directories for each job buys you a little time ~ lol
-----------------------------------------------------------
-----------------------------------------------------------
-----------------------------------------------------------
-----------------------------------------------------------
To add jobs simple add a new project module
-----------------------------------------------------------
-----------------------------------------------------------
-----------------------------------------------------------
-----------------------------------------------------------