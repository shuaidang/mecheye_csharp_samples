using System;
using System.Collections.Generic;
using mmind.apiSharp;

class sample
{
    static void showError(ErrorStatus status)
    {
        if (status.errorCode == (int)ErrorCode.MMIND_STATUS_SUCCESS)
            return;
        Console.WriteLine("Error Code : {0}, Error Description: {1}.", status.errorCode, status.errorDescription);
    }

    static void printDeviceInfo(MechEyeDeviceInfo deviceInfo)
    {
        Console.WriteLine("............................");
        Console.WriteLine("Camera Model Name: {0}", deviceInfo.model);
        Console.WriteLine("Camera ID:         {0}", deviceInfo.id);
        Console.WriteLine("Camera IP Address: {0}", deviceInfo.ipAddress);
        Console.WriteLine("Hardware Version:  V{0}", deviceInfo.hardwareVersion);
        Console.WriteLine("Firmware Version:  V{0}", deviceInfo.firmwareVersion);
        Console.WriteLine("............................");
        Console.WriteLine("");
    }

    static void printDeviceIntri(DeviceIntri intri)
    {
        Console.WriteLine("CameraMatrix: ");
        Console.WriteLine("    [{0}, 0, {1}]", intri.depthCameraIntri.fx, intri.depthCameraIntri.cx);
        Console.WriteLine("    [0, {0}, {1}]", intri.depthCameraIntri.fy, intri.depthCameraIntri.cy);
        Console.WriteLine("    [0, 0, 1]");
        Console.WriteLine("");
        Console.WriteLine("CameraDistCoeffs: ");
        Console.WriteLine("    k1: {0}, k2: {1}, p1: {2}, p2: {3}, k3: {4}", 
            intri.depthCameraIntri.k1, 
            intri.depthCameraIntri.k2, 
            intri.depthCameraIntri.p1, 
            intri.depthCameraIntri.p2, 
            intri.depthCameraIntri.k3);
        Console.WriteLine("");
    }

    static void printDeviceResolution(DeviceResolution deviceResolution)
    {
        Console.WriteLine("Color map size: (width : {0}, height : {1}).", deviceResolution.colorMapWidth, deviceResolution.colorMapHeight);
        Console.WriteLine("Depth map size: (width : {0}, height : {1}).", deviceResolution.depthMapWidth, deviceResolution.depthMapHeight);
    }

    static int Main()
    {
        Console.WriteLine("Find Mech-Eye devices...");
        List<MechEyeDeviceInfo> deviceInfoList = MechEyeDevice.enumerateMechEyeDeviceList();

        if (deviceInfoList.Count == 0)
        {
            Console.WriteLine("No Mech-Eye Device found.");
            return -1;
        }

        for (int i = 0; i < deviceInfoList.Count; ++i)
        {
            Console.WriteLine("Mech-Eye device index : {0}", i);
            printDeviceInfo(deviceInfoList[i]);
        }

        Console.WriteLine("Please enter the device index you want to connect: ");
        int inputIndex = 0;

        while (true)
        {
            string input = Console.ReadLine();
            if (int.TryParse(input, out inputIndex) && inputIndex >= 0 && inputIndex < deviceInfoList.Count)
                break;
            Console.WriteLine("Input invalid! Please enter the device index you wnat to connect: ");
        }

        //MechEyeDeviceInfo deviceInfo = new MechEyeDeviceInfo() { model = "", id = "", hardwareVersion = "", firmwareVersion = "1.5.0", ipAddress = "127.0.0.1", port = 5577 };

        ErrorStatus status = new ErrorStatus();
        MechEyeDevice device = new MechEyeDevice();
        status = device.connect(deviceInfoList[inputIndex]);

        //status = device.connect(deviceInfo);

        if (status.errorCode != (int)ErrorCode.MMIND_STATUS_SUCCESS)
        {
            showError(status);
            return -1;
        }

        Console.WriteLine("Connected to the Mech-Eye device successfully.");

        DeviceIntri deviceIntri = new DeviceIntri();
        showError(device.getDeviceIntri(ref deviceIntri));
        printDeviceIntri(deviceIntri);
        Console.WriteLine(MechEyeDevice.getApiInformation());
        device.disconnect();
        Console.WriteLine("Disconnected from the Mech-Eye device Success.");

        return 0;
    }
}

