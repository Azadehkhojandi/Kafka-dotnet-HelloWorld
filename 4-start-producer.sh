cd kafka_2.11-2.1.0
topic=$1
echo $topic
bin/kafka-console-producer.sh --broker-list localhost:9092 --topic $topic