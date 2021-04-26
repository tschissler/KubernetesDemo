# How to prepare Raspberries to install MicroK8s

In our cluster we have one special Pi called _Head_ and all other nodes which are responsible to form a Kubernetes cluster. We will install Docker on the _Head_ and control the K8s cluster from there. The cluster itself consists of one master and many leaf nodes.

Following steps are to setup all Pis:

## Download and install Ubuntu
1. You should use the 4GB or 8GB version of Raspberry Pi 4. With the 2 GB version we had no success in running MicroK8s
https://www.raspberrypi.org/products/raspberry-pi-4-model-b/?variant=raspberry-pi-4-model-b-4gb
2. Download the Rapberry Imager:
https://www.raspberrypi.org/software/
3. Create an SD card with the latest Ubuntu Server 64Bit image
4. If the node should use Wifi connection, change the network-config file on the SD-card to contain your wifi connection parameters
![image](https://user-images.githubusercontent.com/11467601/114423556-94a39100-9bb7-11eb-8b96-a6d68b0630af.png)
5. Boot the Raspberry from the SD card
6. Login with user `ubuntu` / password `ubuntu` and change the password

## Setup operating system
1. Create a new user (if you would like to have a dedicated user for K8s):
    ```bash
    sudo adduser k8suser
    sudo usermod -aG sudo k8suser
    ```
1. Rename hostname. The hostname `<name>` must be lowercase:
    ```bash
    sudo hostnamectl set-hostname <name>
    ```
1. Reboot the pi:
    ```bash
    sudo reboot
    ```
1. Do package update and install net tools (e.g. for `ifconfig`):
    ```bash
    sudo apt update
    sudo apt upgrade
    sudo apt install net-tools
    ```
1. There is one more setting you have to make before we can install microk8s. We need to enable c-groups so the kubelet will work out of the box. To do this you need to modify the configuration file /boot/firmware/cmdline.txt:
    ```bash
    sudo nano /boot/firmware/cmdline.txt
    ```
    And add the following options:
    `cgroup_enable=memory cgroup_memory=1 `
    
    The file content should be similar to
    ```
    cgroup_enable=memory cgroup_memory=1 net.ifnames=0 dwc_otg.lpm_enable=0 console=serial0,115200 console=tty1 root=LABEL=writable rootfstype=ext4 elevator=deadline rootwait fixrtc
    ```

    Then reboot
    ```bash
    sudo reboot
    ```