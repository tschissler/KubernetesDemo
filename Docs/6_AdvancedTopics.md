# Multi architecture build of docker images
If you want to execute the build process on a amd64 (PC) architecture but run the image on a arm64 (Raspberry Pi) later, the build script has to be changed.

```sh
VERSION=1.7
CLUSTERNAME=a1
sudo docker buildx build --platform linux/arm64 -t $CLUSTERNAME:32000/primedecomposition:$VERSION -f PrimeDecomposition/Dockerfile .
sudo docker buildx build --platform linux/arm64 -t $CLUSTERNAME:32000/numbergenerator:$VERSION -f NumberGenerator/Dockerfile .
sudo docker buildx build --platform linux/arm64 -t $CLUSTERNAME:32000/primedecompositionui:$VERSION -f PrimeDecompositionUi/Dockerfile .
sudo docker push $CLUSTERNAME:32000/primedecomposition:$VERSION && \
sudo docker push $CLUSTERNAME:32000/numbergenerator:$VERSION && \
sudo docker push $CLUSTERNAME:32000/primedecompositionui:$VERSION
```

Source: [Building Multi-Arch Images for Arm and x86 with Docker Desktop](https://www.docker.com/blog/multi-arch-images/)

# Use remote docker host
There are different ways to execute docker commands on a remote host

1. Set the environment variable `DOCKER_HOST`
```bash
DOCKER_HOST=“ssh://user@remotehost” 
```

2. Use a docker context
```bash
docker context create <contextname> ‐‐docker “host=ssh://user@remotemachine”
docker context ls
docker context use <contextname>
docker ps
```

Source: [How to deploy on remote Docker hosts with docker-compose](https://www.docker.com/blog/how-to-deploy-on-remote-docker-hosts-with-docker-compose/)