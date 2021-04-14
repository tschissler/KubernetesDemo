#!/bin/bash

sudo docker build -t a1:32000/primedecomposition:1.0 -f PrimeDecomposition/Dockerfile .
sudo docker build -t a1:32000/numbergenerator:1.0 -f NumberGenerator/Dockerfile .
sudo docker build -t a1:32000/primedecompositionui:1.0 -f PrimeDecompositionUi/Dockerfile .
sudo docker push a1:32000/primedecomposition:1.0 && \
sudo docker push a1:32000/numbergenerator:1.0 && \
sudo docker push a1:32000/primedecompositionui:1.0
