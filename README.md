# IpRangeList
Generate ip list from ip range

For generate ip list:
        IpRangeList.exe [ip range]

        Example of ip ranges:
                192.168.1.0/24
                192.168.1.1-128

                Note: For export to file,use: -export [path]

For generate ip list from file
        IpRangeList.exe -source [path] -export [path]

                Example of file contents (each ip in new line):
                        192.168.1.0/24
                        192.168.1.1-128
