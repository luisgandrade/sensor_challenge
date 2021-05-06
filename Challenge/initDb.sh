for i in {1..50};
do
    /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P '$3nh4-F4c1|' -d master -i SensorLogSchema.sql
    if [ $? -eq 0 ]
    then
        echo "setup.sql completed"
        break
    else
        echo "not ready yet..."
        sleep 1s
    fi
done