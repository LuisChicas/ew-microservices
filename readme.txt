Keeping app alive on production server:

https://jakeydocs.readthedocs.io/en/latest/publishing/linuxproduction.html

---- RELEASE PROCESS ----

1. dotnet publish -c release -r ubuntu.14.04-x64

2. Copy files in "\bin\Release\netcoreapp2.1\ubuntu.14.04-x64\publish" and paste them in server's project folder root.

3. In server's project folder root, run "chmod 755 EasyWalletWeb".

4. Not sure if "sudo service supervisor restart" is needed.

3. dotnet ef migrations script

5. Copy the SQL of the migrations that server's DB doesn't have yet, and run it.