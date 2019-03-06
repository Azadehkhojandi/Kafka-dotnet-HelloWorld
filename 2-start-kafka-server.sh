rm -r /tmp/kafka-logs
cd kafka_2.11-2.1.0
bin/kafka-server-start.sh config/server.properties &
# bin/kafka-server-start.sh config/server-1.properties &
# bin/kafka-server-start.sh config/server-3.properties &