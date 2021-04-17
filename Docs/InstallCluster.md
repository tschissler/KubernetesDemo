# Installing the Cluster

For the Kubernetes cluster we are going to use microk8s (https://microk8s.io/).

Make sure that you ssh-ed into one of your cluster nodes and not running the following commands on your head node.

```bash
ssh k8suser@<node name, e.g. a1>
```

To install microk8s you just need to run a single command:
```bash
sudo snap install microk8s --classic
```

You should run this command on all 3 nodes, e.g. a1, a2 and a3.

After the installation is finished, you can check the status:
```bash
sudo microk8s status --wait-ready
```
This could take a while as it waits untill the services have all started up. 
After a couple of minutes you should see the folloowing output.
If that is not the case try to run `sudo microk8s start` and then `sudo microk8s status --wait-ready` again.

![Screenshot3](Screenshot3.png)

You can authorize the user to run microk8s without using sudo all the time, just to make your live a bit easier :-)
```bash
sudo usermod -a -G microk8s k8suser
sudo chown -f -R k8suser ~/.kube
newgrp microk8s
```

One of your nodes now becomes the master node, the others become leaf nodes. Pick one for master and run the following command:
```bash
sudo microk8s.add-node
```

This command will generate a connection string in the form of `<master_ip>:<port>/<token>` and will provide you with a 
command you can copy over to the other nodes to join the cluster. Be aware that you have to create a new token for each node



