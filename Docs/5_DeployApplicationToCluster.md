# Deploying the application into the cluster

To deploy our application into the cluster, we have to use a different command `kubectl`. 
First make suer that kubectl is installed on your head by running
```bash 
kubectl version
```

![Screenshot_kubectl_version](Screenshot_kubectl_version.png)

## Configure kubectl to connect to the cluster
Next, we have to configure user, cluster and context.
Notes:
* Replace `<master>` with the name of the K8s master node.
* The `<token>` can be fetched on a K8s node with the command `microk8s.kubectl -n kube-system describe secret $(microk8s kubectl -n kube-system get secret | grep default-token | cut -d " " -f1)`
```bash
kubectl config set-credentials user/picluster --token=<token>
kubectl config set-cluster cluster/picluster --insecure-skip-tls-verify=true --server=https://<master_ip>:16443
kubectl config set-context default/picluster/user --user=user/picluster --namespace=default --cluster=cluster/picluster
kubectl config use-context default/picluster/user
```

Let's list the nodes inb our cluster to check the configuration.
```bash
kubectl get nodes
```

![Screenshot_get_nodes](Screenshot_get_nodes.png)


## Deploy applications

First we have to build the container images and deploy them to the cluster repository. 
There is a script available that helps with that. 
You can just look into the `build-and-push-images-on-head.sh` and update the `VERSION` to a new, higher version 
and provide the name of your master node in `CLUSTERNAME`

Then execute the script. 

We have already prepared some configuration
cd ~/KubernetesDemo/.k8s
