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

### Install and configure kubectl on another PI
Install kubectl:
```
curl -LO "https://storage.googleapis.com/kubernetes-release/release/$(curl -s https://storage.googleapis.com/kubernetes-release/release/stable.txt)/bin/linux/arm/kubectl"

mkdir -p ~/bin
mv kubectl ~/bin
chmod a+x ~/bin/kubectl
echo "export PATH=$PATH:~/bin" >> ~/.bashrc
```

=======
```

Configure user, cluster and context:
```
kubectl config set-credentials ubuntu/ClusterA --token=<token>
kubectl config set-cluster ClusterA --insecure-skip-tls-verify=true --server=https://192.168.178.77:16443
kubectl config set-context default/ClusterA/kubeuser --user=ubuntu/ClusterA --namespace=default --cluster=ClusterA
kubectl config use-context default/ClusterA/kubeuser
```

This leads to the following `~/.kube/config`:
```
apiVersion: v1
clusters:
- cluster:
    insecure-skip-tls-verify: true
    server: https://192.168.178.77:16443
  name: ClusterA
contexts:
- context:
    cluster: ClusterA
    namespace: default
    user: ubuntu/clusterA
  name: default/ClusterA/kubeuser
current-context: default/ClusterA/kubeuser
kind: Config
preferences: {}
users:
- name: ubuntu/ClusterA
  user:
    token: REDACTED
```


Install Docker on head:
```
sudo apt-get update
sudo apt-get install     apt-transport-https     ca-certificates     curl     gnupg     lsb-release
curl -fsSL https://download.docker.com/linux/debian/gpg | sudo gpg --dearmor -o /usr/share/keyrings/docker-archive-keyring.gpg
echo   "deb [arch=armhf signed-by=/usr/share/keyrings/docker-archive-keyring.gpg] https://download.docker.com/linux/debian \
  $(lsb_release -cs) stable" | sudo tee /etc/apt/sources.list.d/docker.list > /dev/null
sudo apt-get update
df -h
sudo apt-get install docker-ce docker-ce-cli containerd.io
```

Register our custom Portainer registry with Microk8s:
```
vi /var/snap/microk8s/current/args/containerd-template.toml
```
and add:
```
      [plugins."io.containerd.grpc.v1.cri".registry.mirrors."a1:32000"]
        endpoint = ["http://a1:32000"]
```
at the end of the file.
=======

## Use internal registry
https://microk8s.io/docs/registry-built-in

https://microk8s.io/docs/registry-private

