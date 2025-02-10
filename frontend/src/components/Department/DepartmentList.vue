<template>
  <div class="department-container">
    <div class="header">
      <h2>部門列表</h2>
      <button class="btn btn-primary add-button" @click="handleAddDepartment">
        新增部門
      </button>
    </div>

    <!-- 部門樹狀結構 -->
    <DepartmentTree :departments="departments" @edit="handleEditDepartment" />

    <!-- 新增/編輯部門 Popup -->
    <DepartmentForm
      v-if="showForm"
      :department="selectedDepartment"
      :departments="allDepartments"
      @submit="handleSaveDepartment"
      @close="showForm = false"
    />
  </div>
</template>

<script>
import { ref, computed } from "vue";
import DepartmentTree from "./DepartmentTree.vue";
import DepartmentForm from "./DepartmentForm.vue";

export default {
  name: "DepartmentList",
  components: {
    DepartmentTree,
    DepartmentForm,
  },
  setup() {
    const departments = ref([
      {
        id: 1,
        name: "總公司",
        parentId: null,
        children: [
          {
            id: 2,
            name: "人事部",
            parentId: 1,
            children: [{ id: 4, name: "IT 部門", parentId: 2, children: [] }],
          },
          { id: 3, name: "財務部", parentId: 1, children: [] },
        ],
      },
    ]);

    // **獨立存放所有部門（用於選擇上層部門）**
    const allDepartments = computed(() => {
      const flatList = [];
      const flatten = (deptList) => {
        deptList.forEach((dept) => {
          flatList.push({
            id: dept.id,
            name: dept.name,
            parentId: dept.parentId,
          });
          if (dept.children?.length) {
            flatten(dept.children);
          }
        });
      };
      flatten(departments.value);
      return flatList;
    });

    const showForm = ref(false);
    const selectedDepartment = ref(null);

    const handleEditDepartment = (department) => {
      selectedDepartment.value = department;
      showForm.value = true;
    };

    const handleAddDepartment = () => {
      selectedDepartment.value = null;
      showForm.value = true;
    };

    const handleSaveDepartment = (department) => {
      if (department.id) {
        // 編輯部門
        const dept = allDepartments.value.find((d) => d.id === department.id);
        if (dept) {
          dept.name = department.name;
          dept.parentId = department.parentId;
        }
      } else {
        // 新增部門
        const newDept = {
          id: Date.now(),
          name: department.name,
          parentId: department.parentId,
          children: [],
        };
        allDepartments.value.push(newDept);

        if (newDept.parentId) {
          const parentDept = allDepartments.value.find(
            (d) => d.id === newDept.parentId
          );
          if (parentDept) {
            parentDept.children.push(newDept);
          }
        } else {
          departments.value.push(newDept);
        }
      }

      showForm.value = false;
    };

    return {
      departments,
      allDepartments,
      showForm,
      selectedDepartment,
      handleEditDepartment,
      handleAddDepartment,
      handleSaveDepartment,
    };
  },
};
</script>

<style scoped>
.department-container {
  padding: 20px;
  background: #ffffff;
  border-radius: 8px;
}

.header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 10px;
}

.add-button {
  background-color: #007bff;
  color: white;
  border: none;
  padding: 8px 12px;
  border-radius: 4px;
  cursor: pointer;
}

.add-button:hover {
  background-color: #0056b3;
}
</style>
