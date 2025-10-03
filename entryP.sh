#!/bin/bash
set -e

echo "==> Starting SQL Server..."
/opt/mssql/bin/sqlservr &

echo "==> Waiting for SQL Server to start..."
sleep 40s

echo "==> Running initDatabases.sql..."
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "Strong!Passw0rd" -d master -i /initDatabases.sql || echo "sqlcmd FAILED!"

echo "==> Finished entrypoint script."
wait