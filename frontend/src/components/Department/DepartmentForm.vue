<template>
  <div class="modal-overlay">
    <div class="modal-content">
      <h3>{{ department?.id ? "編輯部門" : "新增部門" }}</h3>

      <label>部門名稱：</label>
      <input type="text" v-model="formData.name" />

      <label>上層部門：</label>
      <select v-model="formData.parentId">
        <option :value="null">無</option>
        <option v-for="dept in filteredDepartments" :key="dept.id" :value="dept.id">
          {{ dept.name }}
        </option>
      </select>

      <div class="modal-actions">
        <button class="btn btn-primary" @click="handleSubmit">儲存</button>
        <button class="btn btn-secondary" @click="$emit('close')">取消</button>
      </div>
    </div>
  </div>
</template>

<script>
import { computed, reactive, watchEffect } from "vue";

export default {
  name: "DepartmentForm",
  props: {
    department: {
      type: Object,
      default: null,
    },
    departments: {
      type: Array,
      required: true,
    },
  },
  setup(props, { emit }) {
    const formData = reactive({
      id: props.department?.id || null,
      name: props.department?.name || "",
      parentId: props.department?.parentId || null,
    });

    // 過濾可選的上層部門
    const filteredDepartments = computed(() => {
      return props.departments.filter((dept) => dept.id !== formData.id);
    });

    // 監聽 `department` 變化並同步更新 `formData`
    watchEffect(() => {
      formData.id = props.department?.id || null;
      formData.name = props.department?.name || "";
      formData.parentId = props.department?.parentId || null;
    });

    const handleSubmit = () => {
      emit("submit", { ...formData });
    };

    return {
      formData,
      filteredDepartments,
      handleSubmit,
    };
  },
};
</script>

<style scoped>
.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background: rgba(0, 0, 0, 0.5);
  display: flex;
  justify-content: center;
  align-items: center;
}

.modal-content {
  background: white;
  padding: 20px;
  border-radius: 8px;
  width: 300px;
}

.modal-actions {
  margin-top: 20px;
  display: flex;
  justify-content: space-between;
}

.btn-primary {
  background-color: #007bff;
  color: white;
  border: none;
  padding: 8px 12px;
  border-radius: 4px;
  cursor: pointer;
}

.btn-primary:hover {
  background-color: #0056b3;
}

.btn-secondary {
  background-color: #6c757d;
  color: white;
  border: none;
  padding: 8px 12px;
  border-radius: 4px;
  cursor: pointer;
}

.btn-secondary:hover {
  background-color: #5a6268;
}
</style>
