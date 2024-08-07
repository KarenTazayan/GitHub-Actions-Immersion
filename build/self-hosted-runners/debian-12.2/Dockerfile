FROM debian:12.2-slim

ENV GITHUB_RUNNER_LABELS="azure-cli,dotnet,resharper-cli"
ENV GITHUB_RUNNER_GROUP="Default"
ENV GITHUB_RUNNER_WORKDIR="_work"

RUN DEBIAN_FRONTEND=noninteractive apt-get update
RUN DEBIAN_FRONTEND=noninteractive apt-get upgrade -y

RUN DEBIAN_FRONTEND=noninteractive apt-get install -y -qq --no-install-recommends \
    apt-transport-https \
    apt-utils \
    ca-certificates \
    curl \
    git \
    iputils-ping \
    jq \
    gnupg \
    lsb-release \
    software-properties-common

# Install the Azure CLI
# RUN curl -sL https://aka.ms/InstallAzureCLIDeb | bash
WORKDIR /azcli
COPY ./az_cli_install.sh .
RUN chmod +x az_cli_install.sh
RUN ./az_cli_install.sh
ENV AZURE_CLI=True

# Install the .NET SDK 8.0
RUN apt-get install wget
RUN wget https://packages.microsoft.com/config/debian/10/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
RUN dpkg -i packages-microsoft-prod.deb
RUN rm packages-microsoft-prod.deb
RUN apt-get update && apt-get install -y dotnet-sdk-8.0

# https://blog.jetbrains.com/dotnet/2020/07/16/resharper-command-line-tools-cross-platform-and-global-tools/#usage-as-global-tools
RUN dotnet tool install JetBrains.ReSharper.GlobalTools --tool-path /resharpercli --version 2024.1.4

# Install PowerShell
RUN apt-get install -y powershell

# Install Docker Engine on Debian
RUN install -m 0755 -d /etc/apt/keyrings
RUN curl -fsSL https://download.docker.com/linux/debian/gpg | gpg --dearmor -o /etc/apt/keyrings/docker.gpg
RUN chmod a+r /etc/apt/keyrings/docker.gpg
# Add the repository to Apt sources:
RUN echo \
  "deb [arch="$(dpkg --print-architecture)" signed-by=/etc/apt/keyrings/docker.gpg] https://download.docker.com/linux/debian \
  "$(. /etc/os-release && echo "$VERSION_CODENAME")" stable" | \
  tee /etc/apt/sources.list.d/docker.list > /dev/null

RUN apt-get update
RUN yes Y | apt-get install docker-ce docker-ce-cli containerd.io docker-buildx-plugin docker-compose-plugin

# Can be 'linux-x64', 'linux-arm64', 'linux-arm', 'rhel.6-x64'.
ENV TARGETARCH=linux-x64

# Actions Runner
WORKDIR /acr

# Download the latest runner package
RUN curl -o actions-runner-linux-x64-2.317.0.tar.gz \
  -L https://github.com/actions/runner/releases/download/v2.317.0/actions-runner-linux-x64-2.317.0.tar.gz
RUN tar xzf ./actions-runner-linux-x64-2.317.0.tar.gz

# Run a self-hosted Azure Pipelines agent in Docker
COPY ./start.sh .
RUN chmod +x start.sh

# Switch to the acru1 user
#USER acru1

ENTRYPOINT [ "./start.sh" ]