import React, { Component } from 'react';
import { DataManager, UrlAdaptor } from '@syncfusion/ej2-data';
import { ColumnDirective, ColumnsDirective, GridComponent } from '@syncfusion/ej2-react-grids';
import { ButtonComponent } from '@syncfusion/ej2-react-buttons';
import { Inject, VirtualScroll, Toolbar, Sort, Group, Filter } from '@syncfusion/ej2-react-grids';

var pageSet = 1;  // defines the pageSet. Initially the Grid renders with first pageSet so the value is 1
var maxRecordsPerPageSet = 500000; // defines the maximum records per pageSet
var currentRecordsPerPage; // available records in the pageSet
var totalRecords; // total records bound to the Grid
var gridPageSize = 50; // pageSize of the Grid component

export class CustomUrlAdaptor extends UrlAdaptor {
    processQuery(args) {
        if (arguments[1].queries) {
            for (var i = 0; i < arguments[1].queries.length; i++) {
                if (arguments[1].queries[i].fn === 'onPage') {
                    // customize the pageIndex based on the current pageSet (It send the skip query inlcuding the previous pageSet )
                    arguments[1].queries[i].e.pageIndex = (((pageSet - 1) * maxRecordsPerPageSet) / gridPageSize) + arguments[1].queries[i].e.pageIndex;
                }
            }
        }
        // Executing base class processQuery function
        var original = super.processQuery.apply(this, arguments);
        return original;
    }
}

export class Home extends Component {

    constructor(props) {
        super(props);
        this.dataManager = new DataManager({
            adaptor: new CustomUrlAdaptor(),
            url: "Home/UrlDatasource"
        });
        this.pageSettings = { pageSize: gridPageSize };
    }

    /** Handling the Grid to interact with particular pageSet **/

    beforeDataBound(args) {
        // storing the total records count
        totalRecords = args.count;
        // change the count with respect to maxRecordsPerPageSet
        args.count = args.count - ((pageSet - 1) * maxRecordsPerPageSet) > maxRecordsPerPageSet ? maxRecordsPerPageSet : args.count - ((pageSet - 1) * maxRecordsPerPageSet);
        currentRecordsPerPage = args.count;
        this.enableDisableMoreOpt();
    }

    actionBegin(args) {
        // change the pageSet to 1 (initial page) when performing data actions
        if (args.requestType === 'sorting' || args.requestType === 'filtering' || args.requestType === 'searching' || args.requestType === 'grouping' || args.requestType === 'ungrouping' ) {
            pageSet = 1;
            this.disableMoreOpt();
        }
    }

    // triggered when clicking the Previous/ Next button
    prevNxtBtnClick(args) {
        if (this.grid.element.querySelector('.e-content') && this.grid.element.querySelector('.e-content').getAttribute('aria-busy') === 'false') {
            this.disableMoreOpt();
            // increase/decrease the pageSet based on the target element
            pageSet = args.target.classList.contains('prevbtn') ? --pageSet : ++pageSet;
            this.rerenderGrid(); // re-render the Grid component
        }
    }

    // disable/ enable the Previous & Next buttons based on the pageSet 
    enableDisableMoreOpt() {
        var nxtBtn = document.getElementsByClassName("nxtbtn")[0].ej2_instances[0];
        var prevBtn = document.getElementsByClassName("prevbtn")[0].ej2_instances[0];
        if (pageSet > 1) {
            prevBtn.element.disabled = false;
        } else {
            prevBtn.element.disabled = true;
        }
        if (totalRecords > pageSet * maxRecordsPerPageSet) {
            nxtBtn.element.disabled = false;
        } else {
            nxtBtn.element.disabled = true;
        }
    }

    // disable the Previous and Next buttons 
    disableMoreOpt() {
        var nxtBtn = document.getElementsByClassName("nxtbtn")[0].ej2_instances[0];
        var prevBtn = document.getElementsByClassName("prevbtn")[0].ej2_instances[0];
        prevBtn.element.disabled = true;
        nxtBtn.element.disabled = true;
    }

    // rerender the Grid component
    rerenderGrid() {
        this.grid.setProperties({
            pageSettings: { currentPage: 1 },
        }, true)
        this.grid.freezeRefresh();
    }

    /** Defining the column templates and customizing it in queryCellInfo event **/

    coltemplate(props) {
        return (<div className="Mapimage">
            <img src="https://ej2.syncfusion.com/react/demos/src/grid/images/Map.png" className="e-image" alt="ShipCountry" />
            <span className="locationtext">{props.ShipCountry}</span>
        </div>);
    }
    statusTemplate(props) {
        return (<div className="statustemp status">
            <span className="statustxt">{props.Status}</span>
        </div>);
    }
    trustTemplate(props) {
        var src = "https://ej2.syncfusion.com/react/demos/src/grid/images/" + props.Trustworthiness + ".png"
        return (<div> <img style={{ width: '31px', height: '18px' }} src={src} alt="Trustworthiness" />
            <span id="Trusttext">{props.Trustworthiness}</span></div>);
    }
    ratingTemplate(props) {
        return (<div className="rating">
            <span className="star"></span>
            <span className="star"></span>
            <span className="star"></span>
            <span className="star"></span>
            <span className="star"></span>
        </div>);
    }
    progressTemplate(props) {
        return (<div className="pbar myProgress">
            <div className="bar myBar">
                <div className="barlabel label"></div>
            </div>
        </div>);
    }
    onQueryCellInfo(args) {
        if (args.column.field === 'Status') {
            if (args.cell.textContent === "Active") {
                args.cell.querySelector(".statustxt").classList.add("e-activecolor");
                args.cell.querySelector(".statustemp").classList.add("e-activecolor");
            }
            if (args.cell.textContent === "Inactive") {
                args.cell.querySelector(".statustxt").classList.add("e-inactivecolor");
                args.cell.querySelector(".statustemp").classList.add("e-inactivecolor");
            }
        }
        if (args.column.field === 'Rating') {
            var span = args.cell.querySelectorAll("span");
            for (var i = 0; i < args.data.Rating; i++) {
                span[i].classList.add("checked");
            }
        }
        if (args.column.field === "Software") {
            if (args.data.Software <= 20) {
                args.data.Software = args.data.Software + 30;
            }
            args.cell.querySelector(".bar").style.width = args.data.Software + "%";
            args.cell.querySelector(".barlabel").textContent = args.data.Software + "%";
            if (args.data.Status === "Inactive") {
                args.cell.querySelector(".bar").classList.add("progressdisable");
            }
        }
    }

    render() {
        this.onQueryCellInfo = this.onQueryCellInfo.bind(this);
        this.beforeDataBound = this.beforeDataBound.bind(this);
        this.actionBegin = this.actionBegin.bind(this);
        this.prevNxtBtnClick = this.prevNxtBtnClick.bind(this);
        return (
            <div>
                <div className="pagearea1">
                    <ButtonComponent cssClass='e-info prevbtn' onClick={this.prevNxtBtnClick} style={{ width: '100%' }}>Load Previous Set...</ButtonComponent>
                </div>
                <GridComponent id='grid' ref={g => this.grid = g} dataSource={this.dataManager} rowHeight={30} enableVirtualization={true} loadingIndicator={{ indicatorType: 'Shimmer' }} pageSettings={this.pageSettings} allowFiltering={true} allowSorting={true} height={330} queryCellInfo={this.onQueryCellInfo} beforeDataBound={this.beforeDataBound} actionBegin={this.actionBegin}>
                    <ColumnsDirective>
                        <ColumnDirective field='OrderID' headerText='Order ID' width='120' textAlign='Right' />
                        <ColumnDirective field='CustomerID' headerText='Customer ID' width='150' />
                        <ColumnDirective field='Freight' format="C2" width='100' textAlign='Right' />
                        <ColumnDirective field='ShipCountry' headerText='Country' width='140' template={this.coltemplate}></ColumnDirective>
                        <ColumnDirective field='Status' headerText='Status' template={this.statusTemplate} width='150' />
                        <ColumnDirective field='Trustworthiness' headerText='Trustworthiness' template={this.trustTemplate} width='160'></ColumnDirective>
                        <ColumnDirective field='Rating' headerText='Rating' template={this.ratingTemplate} width='150' />
                        <ColumnDirective field='Software' allowFiltering={false} allowSorting={false} headerText='Software Proficiency' width='180' template={this.progressTemplate} />
                        <ColumnDirective field='ShipAddress' headerText='Ship Address' width='150' />
                        <ColumnDirective field='ShipName' headerText='Ship Name' width='150' />
                    </ColumnsDirective>
                    <Inject services={[Toolbar, VirtualScroll, Sort, Filter, Group]} />
                </GridComponent>
                <div className="pagearea2">
                    <ButtonComponent cssClass='e-info nxtbtn' onClick={this.prevNxtBtnClick} style={{ width: '100%' }}>Load Next Set...</ButtonComponent>
                </div>
            </div>
        );
    }
}
