#!/bin/bash

VERSION=1.4
CLUSTERNAME=a1
sudo docker build -t $CLUSTERNAME:32000/primedecomposition:$VERSION -f PrimeDecomposition/Dockerfile .
sudo docker build -t $CLUSTERNAME:32000/numbergenerator:$VERSION -f NumberGenerator/Dockerfile .
sudo docker build -t $CLUSTERNAME:32000/primedecompositionui:$VERSION -f PrimeDecompositionUi/Dockerfile .
sudo docker push $CLUSTERNAME:32000/primedecomposition:$VERSION && \
sudo docker push $CLUSTERNAME:32000/numbergenerator:$VERSION && \
sudo docker push $CLUSTERNAME:32000/primedecompositionui:$VERSION
