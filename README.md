# wordcount

Architecture description: https://docs.google.com/drawings/d/1hWMEGFNbf23GlqpfY7TumewMDUJiy8J9H_dUiUKjVw4/edit?usp=sharing

Folder structure:
- WordCount - the app itself
- WordCount.Tests - unit/integration tests for the app
- WordCount.RealTest - testing with some real volumes of data

# Known limitations

The app stores entire frequency dictionary in memory, which means it can run out of it if input volume is large enough. It consumes around 1 GB of RAM for the input size of 128 MB. In the case of huge input volume, the survival of the app depends on two factors: 1) the process is 64 bit, so we won't run of address space, and 2) the system either has enough RAM or has the pagefile enabled. In order to build an app that woudn't have such limitations, more advanced techniques should have been employed, such as storing frequency dictionaries in parts on the disk, merge them using K-way merge algorithm and then perform external sorting.
