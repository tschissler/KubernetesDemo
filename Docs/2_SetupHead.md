# Setup Head Node

## Install Docker
You have to make the descision between `docker.io` (made by the Ubuntu maintainers) and `docker-ce` (made by the Docker developers) to install Docker. Both are viable options and it comes down to preference. See also [this Stackoverflow article](https://stackoverflow.com/questions/45023363/what-is-docker-io-in-relation-to-docker-ce-and-docker-ee) for a brief introduction into the topic.

* To install `docker.io`:
    ```bash
    sudo apt install docker.io
    sudo systemctl enable --now docker
    sudo usermod -aG docker $USER
    ```
* To install `docker-ce`:
    ```bash
    sudo apt-get install apt-transport-https ca-certificates curl gnupg lsb-release
    curl -fsSL https://download.docker.com/linux/ubuntu/gpg | sudo gpg --dearmor -o /usr/share/keyrings/docker-archive-keyring.gpg
    echo "deb [arch=arm64 signed-by=/usr/share/keyrings/docker-archive-keyring.gpg] https://download.docker.com/linux/ubuntu $(lsb_release -cs) stable" | sudo tee /etc/apt/sources.list.d/docker.list > /dev/null
    sudo apt-get update
    sudo apt-get install docker-ce docker-ce-cli containerd.io
    sudo usermod -aG docker $USER
    ```

Logout and login again and you can execute docker commands like this hello world example:
```bash
docker run --rm hello-world && docker rmi hello-world
```

<!-- NOTE FROM MICHAEL: I DIDN'T HAVE TO DO THIS, THERE ALREADY WAS libseccomp2:arm64 (2.5.1-1ubuntu1~20.04.1) INSTALLED
Next you have to update `libseccomp` manually as it will otherwise throw an error when you run `apt update` within a container.
This problem also fixed the issue that the `dotnet` command didn't work in container for .NET6.
```bash
curl http://ftp.us.debian.org/debian/pool/main/libs/libseccomp/libseccomp2_2.5.1-1_armhf.deb --output libseccomp2_2.5.1-1_armhf.deb
sudo dpkg -i libseccomp2_2.5.1-1_armhf.deb
``` -->


## Install VSCode
```
sudo apt install code
```

## Install docker-compose
```bash
sudo apt-get install -y libffi-dev libssl-dev python3 python3-pip
sudo apt-get install -y python3 python3-pip
sudo pip3 install docker-compose
```
Logout and login again to let the PATH extension to take effect. Then `docker-compose` is available.

## Install and configure kubectl on Head
```bash
mkdir -p ~/bin
curl -L "https://storage.googleapis.com/kubernetes-release/release/$(curl -s https://storage.googleapis.com/kubernetes-release/release/stable.txt)/bin/linux/arm/kubectl" -o ~/bin/kubectl
chmod a+x ~/bin/kubectl
echo "export PATH=$PATH:~/bin" >> ~/.bashrc
```
Logout and login again to let the PATH extension to take effect.
After that configure user, cluster and context.
Notes:
* Replace `<master_ip>` with the IP address of the K8s master node.
* The `<token>` can be fetched on a K8s node with the command `microk8s.kubectl -n kube-system describe secret $(microk8s kubectl -n kube-system get secret | grep default-token | cut -d " " -f1)`
```bash
kubectl config set-credentials user/picluster --token=<token>
kubectl config set-cluster cluster/picluster --insecure-skip-tls-verify=true --server=https://<master_ip>:16443
kubectl config set-context default/picluster/user --user=user/picluster --namespace=default --cluster=cluster/picluster
kubectl config use-context default/picluster/user
```

## Configure Docker
To be able to push to the repo from a docker client, you need to add the repo as an insecure repo.
Under Windows, open Docker UI => Settings => Docker Engine. Under Linux edit (or create) `/etc/docker/daemon.json`:
```json
{
  "insecure-registries": ["<cluster_ip>:32000"]
}
```
at the end of the file. Replace `<cluster_ip>` with the IP of one of the cluster nodes' IP address. Restart Docker for the changes to take effect:
```bash
sudo service docker restart
```
