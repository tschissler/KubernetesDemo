# Installing the Cluster
For the Kubernetes cluster we are going to use `microk8s` (https://microk8s.io/). This walkthrough was inspired by https://ubuntu.com/tutorials/how-to-kubernetes-cluster-on-raspberry-pi#1-overview.

Make sure that you ssh-ed into one of your cluster nodes and not running the following commands on your _Head_ node.

## Install `microk8s`
You just need to run a single command:
  ```bash
  sudo snap install microk8s --classic
  ```

You should run this command on all 3 nodes.

After the installation is finished, you can start the node with:
  ```bash
  sudo microk8s start
  ```

and check the status with:
  ```bash
  sudo microk8s status --wait-ready
  ```

This could take a while as it waits until the services have all started up. 
After a couple of minutes you should see the folloowing output.
If that is not the case try to run `sudo microk8s start` and then `sudo microk8s status --wait-ready` again.

![Screenshot3](Screenshot3.png)

You can authorize the user to run microk8s without using sudo all the time, just to make your live a bit easier :-)
```bash
sudo usermod -a -G microk8s $USER
sudo chown -f -R $USER ~/.kube
```

## Assign master and leafs
One of your nodes now becomes the master node, the others become leaf nodes. Pick one for master and run the following command:
  ```bash
  sudo microk8s.add-node
  ```
This command will generate a connection string in the form of `<master_ip>:<port>/<token>` and will provide you with a 
command you can copy over to one leaf node to join the cluster. Be aware that you have to create a new token for each node
but you can only add another leaf node after the first has connected successfully.

You can list the registered nodes with
  ```bash
microk8s kubectl get nodes
```

## Enable microk8s plugins
```bash
microk8s.enable dashboard dns metrics-server portainer registry
```

Register our custom Portainer registry with Microk8s, so execute this *on every* cluster node:
```bash
sudo nano /var/snap/microk8s/current/args/containerd-template.toml
```
and add in the end of the file:
```
      [plugins."io.containerd.grpc.v1.cri".registry.mirrors."<cluster_ip>:32000"]
        endpoint = ["http://<cluster_ip>:32000"]
```
Replace `<cluster_ip>` with the IP of one of the cluster nodes' IP address.
Restart the cluster for the changes to take effect:
```bash
microk8s.stop
microk8s.start
```
Again, you have to do this on every node.

More information about the private registry:
* https://microk8s.io/docs/registry-built-in
* https://microk8s.io/docs/registry-private


## Start the K8s dashboard
The [microk8s dashboard addon](https://microk8s.io/docs/addon-dashboard) is a website for monitoring and controlling the K8s cluster. After enabling the addon it already runs in a container in the cluster and just needs to be assigend to a port on a cluster node to reach it:
```bash
microk8s kubectl port-forward -n kube-system service/kubernetes-dashboard 10443:443 --address 0.0.0.0
```
Now you can open the webpage `https://<ip>:10443` with the `<ip>` of the cluster node you're running the command. After ignoring the certificate warning, you have to authorize. You can get the token by executing the following command on one of the cluster nodes:
```bash
microk8s.kubectl -n kube-system describe secret $(microk8s kubectl -n kube-system get secret | grep default-token | cut -d " " -f1)
```

