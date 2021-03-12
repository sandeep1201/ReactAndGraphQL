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
Silambholi.THolkappian@wisconsin.gov

DCFDLWWPBITSSupport@wisconsin.gov

- overview
	- Batch runs under Cntl-M
	- written as .NET wrappers
	
- naming conventions
	- in Cntl-M
	- in .Net
	- traceback Cntl-M --> .NET/.NET classes
	
- output
	- log as *.log
	- results as *.csv
	- console (for Ctrl-M)
	
- program structure - three pieces
	- object-oriented (shouldn't have to explain that)
	- input (sprocs / results fetching)
	- output generation
	- flow control

	- uses Dependency Injection (Autofac)
	- separation of concerns
	- easier maintenance (encapsulation, traceback)
	- easier to add batch jobs
	
	- creating new jobs
		- create the new classes (IBatchJob, JobName.cs)
		- register the classes with container (Autofac)
		- test it
		- don't forget to setup Cntl-M (job def. && scheduling (off-topic))

- program creates container
	- container injects input class and output generation class
	- runs the job, which returns sproc resultset
	- feeds the resultset to the export/formatting class
	- that's it
-----------------------------------------------------------
-----------------------------------------------------------
-----------------------------------------------------------
-----------------------------------------------------------
-----------------------------------------------------------
-----------------------------------------------------------