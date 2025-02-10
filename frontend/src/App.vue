<template>
  <div id="app">
    <NavBar v-if="!$route.meta.hidNav" />
    <router-view />
  </div>
</template>

<script setup>
import NavBar from "./components/NavBar.vue";
</script>

<style></style>
