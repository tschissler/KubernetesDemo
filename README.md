# KubernetesDemo
A demo application to demonstrate features of Kubernetes

This application does a very simple prime-decomposition.

## Helpful commands and links
Install MicroK8s on a Raspberry Pi:
https://ubuntu.com/tutorials/how-to-kubernetes-cluster-on-raspberry-pi#1-overview

Create new user
```
sudo adduser k8suser
sudo usermod -aG sudo k8suser
```

Rename Computer
```
sudo nano /etc/hostname
sudo hostname -F /etc/hostname
```

Enable Dashboard:
https://microk8s.io/docs/addon-dashboard
```
microk8s kubectl port-forward -n kube-system service/kubernetes-dashboard 10443:443 --address 0.0.0.0
```
