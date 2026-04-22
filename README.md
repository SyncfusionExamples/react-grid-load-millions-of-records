# How to Overcome the Browser Height Limitation in Virtual Scrolling

## Repository Description

This repository contains a sample application that demonstrates practical solutions to overcome browser height limitations when using virtual scrolling in the Syncfusion Grid with large datasets.

## Project Overview

Virtual scrolling is commonly used to efficiently render large volumes of data by loading rows dynamically as the user scrolls. However, browsers impose height limitations that can affect virtualization when extremely large datasets are rendered. This sample illustrates how to handle such scenarios by customizing paging logic and data requests to work around browser height constraints.

The project combines an ASP.NET Core backend with a React-based Syncfusion Grid frontend. On the server side, large datasets are generated and processed using query operations such as paging, sorting, filtering, and searching. On the client side, a custom URL adaptor and page set logic are implemented to manage virtual scrolling beyond typical browser limits while maintaining smooth user interaction.

## Key Features

- Demonstrates virtual scrolling with very large datasets
- Overcomes browser height limitations using custom page set logic
- Server-side data operations for paging, sorting, and filtering
- ASP.NET Core controller integration with Syncfusion Grid
- React Grid implementation with virtualization enabled

## Prerequisites

- Visual Studio 2022
- ASP.NET Core compatible .NET SDK
- Node.js and npm (for running the React client application)

## Running the Application

Follow the steps below to clone the repository, restore dependencies, and run the application.

1. Clone the repository and navigate to the project directory:

   ```bash
       git clone <repository-url>
       cd browser-height-limitation-virtual-scrolling
   ```

2. Restore the required NuGet packages:

   ```bash
   dotnet restore
   ```

3. Restore client-side dependencies and build the React application:

   ```bash
   cd ClientApp
   npm install
   npm run build
   cd ..
   ```

4. Run the application using the .NET CLI or Visual Studio:

   ```bash
   dotnet run
   ```

After the application starts, launch the displayed URL in a browser. Scroll through the grid to observe how virtual scrolling continues to function smoothly even with very large datasets.

## Usage Notes

This sample uses a custom URL adaptor and page set calculation to load data in manageable chunks. The approach helps avoid browser rendering limitations while still providing a seamless virtual scrolling experience.

## Additional Resources

- [Syncfusion React Grid Virtual Scrolling Documentation](https://ej2.syncfusion.com/react/documentation/grid/scrolling/virtual-scrolling)
- [Syncfusion React Grid Data Binding - UrlAdaptor](https://ej2.syncfusion.com/react/documentation/grid/connecting-to-adaptors/url-adaptor)
- [Syncfusion React Grid Demos](https://ej2.syncfusion.com/react/demos/#/tailwind3/grid/overview)
