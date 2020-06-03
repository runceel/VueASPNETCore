<template>
  
</template>

<script lang="ts">
import Vue from 'vue'
export default Vue.extend({
  created() {
    const returnUrl = this.$route.query.returnUrl;
    if (typeof(returnUrl) === 'string') {
      window.location.replace(returnUrl);
    }
  }
});
</script>

<style>

</style>