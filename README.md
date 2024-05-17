# Google Report Console App

## Overview

This console application interacts with the Google Shopping API to generate various performance reports, including:
- Merchant Performance Reports (Clicks and Impressions for Free Listings, Organic Ads, and Shopping Ads)
- Price Competitiveness Reports
- Price Insights Reports
- Shopping Experience Scorecard
- Promotions

The app gathers data, parses it, and writes detailed reports summarizing the performance and opportunities for improvement.

## Features

- **Data Retrieval**: Fetches detailed performance metrics from the Google Shopping API.
- **Data Parsing**: Processes and interprets the retrieved data to generate insights.
- **Report Generation**: Compiles the parsed data into a comprehensive report and saves it to the user's Downloads folder.

## Setup

### Prerequisites

- .NET Core SDK (version 3.1 or later)
- Google API credentials (merchant ID and refresh token)

### Installation

1. **Clone the repository**:
    ```sh
    git clone https://github.com/yourusername/googlereportconsoleapp.git
    cd googlereportconsoleapp
    ```

2. **Restore dependencies**:
    ```sh
    dotnet restore
    ```

3. **Configure your credentials**:
    - Open `Program.cs`.
    - Set `_merchantId` and `_refreshToken` with your Google API credentials.
    ```csharp
    _merchantId = "your-merchant-id";
    _refreshToken = "your-refresh-token";
    ```

### Running the Application

To run the application, use the following command:
```sh
dotnet run
