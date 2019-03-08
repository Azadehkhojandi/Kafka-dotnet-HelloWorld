cd kafka_2.11-2.1.0

echo 'all topis'
bin/kafka-topics.sh --list --zookeeper localhost:2181 

echo 'all brokers'
bin/zookeeper-shell.sh localhost:2181 <<< "ls /brokers/ids"