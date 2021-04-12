# How to prepare Raspberries to install MicroK8s

Execute each of the following steps for each Rasperry you would like to add to your cluster as a node.

1. You should use the 4GB or 8GB version of Raspberry Pi 4. With the 2 GB version we had no success in running MicroK8s
https://www.raspberrypi.org/products/raspberry-pi-4-model-b/?variant=raspberry-pi-4-model-b-4gb
2. Download the Rapberry Imager:
https://www.raspberrypi.org/software/
3. Create an SD card with the latest Ubuntu Server 64Bit image
4. If the node should use Wifi connection, change the network-config file on the SD-card to contain your wifi connection parameters
![image](https://user-images.githubusercontent.com/11467601/114423556-94a39100-9bb7-11eb-8b96-a6d68b0630af.png)
5. Boot the Raspberry from the SD card
6. Login with user ubuntu / password ubuntu and change the password
7. Create a new user
```bash
sudo adduser k8suser
sudo usermod -aG sudo k8suser
```
8. Rename hostname

The hostname must be lowercase
```bash
sudo nano /etc/hostname
sudo hostname -F /etc/hostname
```
9. Enable c-groups
```bash
sudo nano /boot/firmware/cmdline.txt
```
Add the following options at the beginning of the line
```bash
cgroup_enable=memory cgroup_memory=1,
```
Then reboot the pi
```bash
sudo reboot
```
10. Do some helpful preparations
```bash
sudo apt update
sudo apt upgrade
sudo apt install net-tools
sudo apt install code
```
