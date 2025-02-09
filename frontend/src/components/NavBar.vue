<template>
    <nav class="navbar navbar-expand-lg navbar-light bg-light">
        <div class="container-fluid">
            <a class="navbar-brand" href="#">MyApp</a>
            <div class="collapse navbar-collapse">
                <ul class="navbar-nav">
                    <li class="nav-item">
                        <router-link to="/home" class="nav-link">首頁</router-link>
                    </li>
                    <li class="nav-item">
                        <router-link to="/employee" class="nav-link">人員列表</router-link>
                    </li>
                    <li class="nav-item">
                        <router-link to="/department" class="nav-link">部門列表</router-link>
                    </li>
                </ul>
            </div>
            <div class="d-flex align-items-center">
                <span class="navbar-text me-3 fw-bold">👤小明</span>
                <button class="btn btn-outline-danger btn-sm" >登出</button>
            </div>
        </div>
    </nav>
</template>

<script>
    export default {
        name:'NavBar',
        methods: {
            logout() {
                this.$router.push('/login');
            }
        }
    };
</script>