import { createRouter, createWebHistory } from "vue-router";
import LoginForm from "../components/LoginForm.vue";
import HomePage from "../components/HomePage.vue";
import EmployeeList from "../components/EmployeeList.vue";
import DepartmentList from "@/components/Department/DepartmentList.vue";

const routes = [
  { path: "/", redirect: "/login" },
  { path: "/login", component: LoginForm, meta: { hidNav: true } },
  { path: "/home", component: HomePage },
  { path: "/employee", component: EmployeeList },
  { path: "/department", component: DepartmentList },
];

/*
history: 可選 
1.createWebHistory()  HTML5 History 模式
2.createWebHashHistory()  Hash 模式
*/
const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;
