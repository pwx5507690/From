var config = {};
// api 
config.baseApiUrl = "http://localhost:18635/api/";
// temp
config.baseTempUrl = "Client";
// run state debug release
config.state = "debug";
// pagination
config.pagination = 15;
// userToken
config.userTokenName = "userCache";
// page
config.page = {
    "pagebefore": "pagebefore",
    "pageload": "pageload",
    "pageend": "pageend"
};
// http config
config.http = {};
// http method
config.http.method = {
    "get": "get",
    "post": "post",
    "put": "put",
    "delete": "delete",
};
// http return dataType
config.http.dataType = {
    "json": "json",
    "xml": "xml",
    "jsonp": "jsonp",
    "text": "text",
    "html": "html",
    "script": "script"
};
// require config 
config.require = {};
config.require.paths = {
    // js lib
    "jQuery": "../lib/jquery",
    "underscore": "../lib/underscore-min",
    "text": "../lib/text",
    "modal": "../lib/modal.js?v=" + new Date(),
    "pagination": "../lib/pagination.js?v=" + new Date(),
    "table": "../lib/table.js?v=" + new Date(),
    "director": "../lib/director",
    // common
    "app": "app",
    "control": "control",
    "extendObject": "extend",
    "userStorage": "userStorage",
    "run": "run.js?v=23494848",
    // router
    "router": "../routes/router",
    // controller
    "main": "../client/main",
    "dept": "../client/department",
    "menu": "../client/menu",
    "user": "../client/user",
    "role": "../client/role",
    // services
    "baseServices": "../services/baseServices",
    "menuServices": "../services/menuServices",
    "departmentServices": "../services/departmentServices",
    "userServices": "../services/userServices",
    "roleServices": "../services/roleServices"
};
config.require.shim = {
    'underscore': {
        exports: '_'
    },

    'jQuery': {
        exports: 'jQuery'
    },
    'app': {
        deps: ['jQuery'],
    },
    'router': {
        deps: ['director', 'app'],
        exports: 'router'
    },
    'extendObject': {
        deps: ['jQuery', 'app', 'control', 'modal']
    },
    'userStorage': {
        deps: ['jQuery', 'app']
    },
    'baseServices': {
        deps: ['jQuery', 'app']
    },
    'modal': {
        deps: ['jQuery', 'app', 'control']
    },
    'menuServices': {
        deps: ['jQuery', 'baseServices']
    },
    'userServices': {
        deps: ['jQuery', 'baseServices']
    },
    'roleServices': {
        deps: ['jQuery', 'baseServices']
    },
    'dyncDataServices': {
        deps: ['jQuery', 'baseServices']
    },
    'main': {
        deps: ['jQuery', 'menuServices', 'control', 'app']
    },
    'run': {
        deps: ['jQuery', 'app']
    },
    'dept': {
        deps: ['jQuery', 'departmentServices', 'control', 'app', 'table', 'pagination']
    },
    'menu': {
        deps: ['jQuery', 'menuServices', 'control', 'app', 'table', 'pagination']
    },
    'user': {
        deps: ['jQuery', 'userServices', 'roleServices', 'departmentServices', 'control', 'app', 'table', 'pagination']
    },

    'role': {
        deps: ['jQuery', 'roleServices', 'menuServices', 'control', 'app', 'table', 'pagination']
    },

};
config.runPageConfig = [
    {
        view: "dept",
        location: "tempDepartmentQuery",
        controller: "dept",
        type: "query",
        method: "initQueryView",
        storageName: "department",
    },
    {
        view: "deptoption",
        location: "tempDepartmentQuery",
        controller: "dept",
        type: "option",
        method: "initOptionView",
        storageName: "department",
    },
    {
        view: "menuPage",
        location: "tempMenuQuery",
        controller: "menu",
        type: "query",
        method: "initQueryView",
        storageName: "menuQueryPage",
    },
    {
        view: "menuoption",
        location: "tempMenuQuery",
        controller: "menu",
        type: "option",
        method: "initOptionView",
        storageName: "menuQueryPage",
    },
    {
        view: "userQuery",
        location: "tempUserQuery",
        controller: "user",
        type: "query",
        method: "initQueryView",
        storageName: "userQueryPage",
    },
    {
        view: "useroption",
        location: "tempUserQuery",
        controller: "user",
        type: "option",
        method: "initOptionView",
        storageName: "userQueryPage",
    },
    {
        view: "roleQuery",
        location: "tempRoleQuery",
        controller: "role",
        type: "query",
        method: "initQueryView",
        storageName: "roleQueryPage",
    },
    {
        view: "roleoption",
        location: "tempRoleQuery",
        controller: "role",
        type: "option",
        method: "initOptionView",
        storageName: "roleQueryPage",
    }
];
