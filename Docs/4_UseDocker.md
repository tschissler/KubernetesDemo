# How to use Docker to run the app

## Build and run an Docker image

Execute the following commands on the _Head_.

1. Clone the repository for the application
    ```bash
    git clone https://github.com/tschissler/KubernetesDemo.git
    ```
1. Try to run the PrimeDecomposition service
    ```bash
    cd KubernetesDemo/PrimeDecomposition
    dotnet run
    ```
    This results in an error message as the .NET SDK is not installed on the machine.
    ![Screenshot1](Screenshot1.png)
1. We could now install all the dependencies on the local computer to get the app compiling and running. But look, there is a Dockerfile in the current folder. Could we not just create a container and run the build and execution inside the container? We do not have to know much about the needed environment, everything is defined in the Dockerfile. Just try it out first, we will have a look at the file a bit later.
    ```bash
    cd ~/KubernetesDemo
    docker build -f PrimeDecomposition/Dockerfile -t primedecompservice .
    ```
    Make sure you do not miss the dot in the end as it specifies the current folder as the context for the whole build operation. 
    The docker command can only access files within this context. The operation can take a bit if it runs for the first time as it downloads some 
    images from dockerhub, a public repository for docker images. No worries, next time it will be much faster.
    
    The result of this operation should loook something like this.

    ![image](https://user-images.githubusercontent.com/11467601/115914576-6c971600-a472-11eb-9fd3-83b023593bf8.png)

    This command builds a docker image which we tag 'primedecompservice' to refer to it later instead of having to fiddle with IDs.
    You can easily list all available images:
    ```bash
    k8suser@headb:~/KubernetesDemo $ docker images
    REPOSITORY                        TAG       IMAGE ID       CREATED              SIZE
    primedecompservice                latest    caf3d2255a9d   About a minute ago   174MB
    <none>                            <none>    00a7f1273284   About a minute ago   576MB
    mcr.microsoft.com/dotnet/sdk      5.0       e388c04f9eb3   46 hours ago         569MB
    mcr.microsoft.com/dotnet/aspnet   5.0       0d95d6c17320   46 hours ago         174MB
    k8suser@headb:~/KubernetesDemo $ 
    ```
    Here you can see that we have some dotnet images provided by microsoft. The image we just created is based on the last one.

1. You can now run a container using this image.
    ```bash
    docker run -it --rm -p 8080:80 primedecompservice
    ```

This will run our service within the container. We are exposing the port 80 which is used by our service within the container to the local port 8080.
![Screenshot2](Screenshot2.png)

You can now access the service in a browser on the head node: http://localhost:8080/?number=52

That was easy and each time the service need changes in the environment (components, frameworks, environmentvariables etc.) this can be 
considered in the Dockerfile and your can easily create a new container with all these requirements.

Let's have a look at the Dockerfile before we move on. 

```bash
code ~/KubernetesDemo/PrimeDecomposition/Dockerfile 
```

```dockerfile
01  FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
02  COPY Dtos/ /src/Dtos/
03  COPY PrimeDecomposition/ /src/PrimeDecomposition/
04  WORKDIR /src/PrimeDecomposition
05  RUN dotnet restore
06  RUN dotnet publish "PrimeDecomposition.csproj" -c Release -o /app
07  
08  FROM mcr.microsoft.com/dotnet/aspnet:5.0
09  WORKDIR /app
10  COPY --from=build /app .
11  ENV ASPNETCORE_URLS="http://+:80"
12  EXPOSE 80
13  ENTRYPOINT [ "dotnet", "PrimeDecomposition.dll" ]
```

Lines 1-6 are describing the build process. In line 1 the image (which by default is pulled from dockerhub.io, 
a public repository for docker images) is defined and the container is referenced to as "build".
Line 2 and 3 are copying files from the context into the container. Then we change in the folder and 
run commands to build the service. Line 6 does a build and creates an output file (PrimeDecomposition.dll) that
will be executed during runtime. The files will be stored in the /app folder.

Lines 8-13 are describing the image that is created. Here we use a different image which is only including the dotnetcore 
runtime and not the full sdk. In line 10 we copy the files from our build container into the image. Line 11 sets an environment 
variable while line 12 defines the port to expose to the external world. Finally line 13 defines the command to be executed 
when the container is starting up.

## How to create and run multiple containers
OK, that was easy, but our application consists of 3 different services. Obviously we could do the same thing for each service, 
but wouldn't it be cool if we could easily control these containers together? Here comes docker-compose to our rescue.
Fortunately there is already a `docker-compos.yml` file in the root folder of the repository. Again, let's first try it and then look into it.

```bash
cd ~/KubernetesDemo
docker-compose up
```
This now creates images for each of the 3 services and then starts a container for each. You can access the result in a browser on the head again:
`http://localhost:4300`

Not bad, not bad at all. 

With Docker you have no longer worry about the right environment, setting ports, environment variables etc. This is now part of the source code repository. 
This is what we call "Infrastructure as code". No need to create backups of the environments you run your applications in. These can easily be recreated, even on a different hardware. You just have to backup your data.

So let's have a look at the `dockercompose.yml` because there is some nice magic to be discovered.

```yml
version: "3.7"

services:
  prime-decomposition:
    container_name: prime-decomposition
    image: "prime-decomposition"
    build:
      context: .
      dockerfile: ./PrimeDecomposition/Dockerfile
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
    expose:
      - "80"
    networks:
      - k8sdemo

  number-generator:
    container_name: number-generator
    image: "number-generator"
    build:
      context: .
      dockerfile: ./NumberGenerator/Dockerfile
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      - PRIME_DECOMPOSITION_URL=http://prime-decomposition
    expose:
      - "80"
    networks:
      - k8sdemo

  prime-decomposition-ui:
    container_name: prime-decomposition-ui
    image: "prime-decomposition-ui"
    build:
      context: .
      dockerfile: ./PrimeDecompositionUi/Dockerfile
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      - NUMBER_GENERATOR_URL=http://number-generator
    expose:
      - "80"
    ports:
      - "4300:80"
    networks:
      - k8sdemo
        
networks:
  k8sdemo:
```












## Push all images into registry
This step is necessary so that the K8s cluster can find the images. For that, we build the images on the _Head_ and push them into the internal Container Registry running on the cluster.

1.
    ```bash
    cd ~/KubernetesDemo
    chmod a+x ./build-and-push-images-on-head.sh
    ```
1. Update the VERSION and CLUSTERNAME in the file `./build-and-push-images-on-head.sh`
1.
    ```bash
    ./build-and-push-images-on-head.sh
    ```

## Let pods run on the K8s cluster
```bash
cd ~/KubernetesDemo/.k8s
```
Change the image name and version number, i.e.:
```yml
      containers:
        - name: number-generator
          image: <cluster_ip>:32000/numbergenerator:<version>
```
The version needs to be the same as in the `build-and-push-images-on-head.sh` script. After that execute
```bash
kubectl apply -f .
```

By default the Horizontal Autoscaler for `prime-decomposition` is disabled. If you want to activate it execute the command
```bash
kubectl autoscale deployment prime-decomposition --cpu-percent=50 --min=1 --max=12
```
To delete it again run
```bash
kubectl delete horizontalpodautoscaler.autoscaling/prime-decomposition
```
