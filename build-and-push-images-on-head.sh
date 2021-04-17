#!/bin/bash

VERSION=1.4
sudo docker build -t a1:32000/primedecomposition:$VERSION -f PrimeDecomposition/Dockerfile .
sudo docker build -t a1:32000/numbergenerator:$VERSION -f NumberGenerator/Dockerfile .
sudo docker build -t a1:32000/primedecompositionui:$VERSION -f PrimeDecompositionUi/Dockerfile .
sudo docker push a1:32000/primedecomposition:$VERSION && \
sudo docker push a1:32000/numbergenerator:$VERSION && \
sudo docker push a1:32000/primedecompositionui:$VERSION
