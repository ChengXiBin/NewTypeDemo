<template>
  <ul class="department-tree">
    <li v-for="dept in departments" :key="dept.id">
      <div class="department-item">
        <span @click="toggleExpand(dept.id)">
          {{ isExpanded[dept.id] ? "▼" : "▶" }} {{ dept.name }}
        </span>
        <button
          class="btn btn-sm btn-warning ms-2"
          @click="$emit('edit', dept)"
        >
          編輯
        </button>
      </div>

      <!-- 遞迴渲染子部門 -->
      <DepartmentTree
        v-if="dept.children && dept.children.length > 0 && isExpanded[dept.id]"
        :departments="dept.children"
        @edit="$emit('edit', $event)"
        class="child-department"
      />
    </li>
  </ul>
</template>

<script>
import { ref } from "vue";

export default {
  name: "DepartmentTree",
  props: {
    departments: {
      type: Array,
      required: true,
      default: () => [],
    },
  },
  setup() {
    const isExpanded = ref({}); // 使用 Vue 3 的 `ref` 來管理展開狀態

    const toggleExpand = (id) => {
      isExpanded.value[id] = !isExpanded.value[id];
    };

    return {
      isExpanded,
      toggleExpand,
    };
  },
};
</script>

<style scoped>
.department-tree {
  list-style-type: none;
  padding-left: 10px;
}

.department-tree li {
  margin-left: 20px;
  padding: 5px 0;
}

.department-item {
  display: flex;
  align-items: center;
  cursor: pointer;
}

.child-department {
  margin-left: 20px;
}
</style>
