```sh
docker exec -it kafka bash # Exec into the container:

# Create a topic:
kafka-topics \
  --bootstrap-server kafka:29092 \
  --create \
  --topic orders \
  --partitions 1 \
  --replication-factor 1
```
