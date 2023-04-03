# Sat.Recruitment

The web api refactor includes structural design changes that far exceed 2 hours of work.

I have deliberately left out some aspects such as:

1. The persistence in the DB (it works in any local SQL Server with the name of the DB that is in the connection and migrating the User entity)

2. The global control of exceptions through a filter (although the basic structure of the business exceptions is already prepared).

3. The error log (generally I implement it on the same DB and a mail system according to severity).

4. The use of cache (both in memory and distributed).

Initially create a branch on top of the main branch. But since I didn't have permissions to push, I ended up uploading everything to a personal git.

The hourly demand that I have daily in the jobs that I do do not allow me to dedicate more time to this challenge.
