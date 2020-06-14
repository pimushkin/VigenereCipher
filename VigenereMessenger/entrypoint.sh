#!/bin/bash

set -e
run_cmd="dotnet bin/Debug/netcoreapp3.1/VigenereMessenger.dll"

until dotnet ef database update --context ApplicationDbContext; do
>&2 echo "SQL Server is starting up"
sleep 1
done

>&2 echo "SQL Server is up - executing command"
exec $run_cmd